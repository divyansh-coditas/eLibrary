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

        // this action method will return all the user of the application
        public ActionResult Get()
        {
            var data = userdata.Get();
            return View(data);
        }

        public ActionResult Create()
        { 
            return View();
        }

        // this action method is to register new user
        [HttpPost]
        public ActionResult Create(User user)
        {
                user.RoleID = 2;
                // this will check whether the user already exist or not
                bool isValid = userdata.Get().Any(x => x.UserName == user.UserName && x.UserPassword == user.UserPassword);
                if (!isValid)
                {
                    var response = userdata.Create(user);
                    TempData["login"] = 1;
                    return RedirectToAction("Login");
                }

                ViewBag.Message = "User already exist";
                return View();
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
                // this will check the username and password matches or not
                bool isValid = context.Users.Any(u => u.UserName == name && u.UserPassword == password);
                if (isValid)
                {
                    var data = (from us in userdata.Get()
                                where us.UserName == name && us.UserPassword == password
                                select us).ToList();
                    // here storing the userid in Session
                    Session["Id"] = data[0].UserId;
                    FormsAuthentication.SetAuthCookie(name, false);
                    return RedirectToAction("DashBoard");                
                }
                ViewBag.Message = "Invalid Username or Password"; ;    
                return View();
            }
        }

        [Authorize]
        public ActionResult DashBoard() 
        {
            if (Session["Id"] != null)
            {
                var result = userdata.Get().Where(m => m.UserId == Convert.ToInt32(Session["Id"])).FirstOrDefault();
                return View(result);
            }
            else 
            {
                TempData["failed"] = 1;
                return RedirectToAction("Login", "user");
            }
        }
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}