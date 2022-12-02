using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using eLibrary.Services;
using System.Web.Security;
using eLibrary.Models;

namespace eLibrary.Controllers
{
    public class UserController : Controller
    {
        UserDataAccess userdata = new UserDataAccess();
        public ActionResult Get()
        {
            var data = userdata.Get();
            return View(data);
        }

        public ActionResult Create()
        {
            var User = new User();
            return View(User);
        }

        [HttpPost]

        public ActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                user.RoleID = 2;
                bool isValid = userdata.Get().Any(x => x.UserName == user.UserName && x.UserPassword == user.UserPassword);
                if (!isValid)
                {
                    var response = userdata.Create(user);
                    return RedirectToAction("Login");
                }
                else 
                {
                    return View();
                }
            }
            else
            {
                ModelState.AddModelError("", "UserName Already Taken");
                return View();
            }
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string name, string password)
        {
            using (var context = new eLibraryEntities())
            {
                bool isValid = context.Users.Any(u => u.UserName == name && u.UserPassword == password);
                if (isValid)
                {
                    var data = (from us in userdata.Get()
                                where us.UserName == name && us.UserPassword == password
                                select us).ToList();
                    Session["Id"] = data[0].UserId;
                    FormsAuthentication.SetAuthCookie(name, false);
                    return RedirectToAction("DashBoard");                
                }
                ModelState.AddModelError("", "Invalid username or password");
                
                return View();
            }
        }
        public ActionResult DashBoard() 
        {
            var result = userdata.Get().Where(m => m.UserId == Convert.ToInt32(Session["Id"])).FirstOrDefault();
            return View(result);
        }
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}