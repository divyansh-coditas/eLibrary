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
         UserRoleProvider role = new UserRoleProvider();

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
            user.RoleID = 2;
            bool isValid = userdata.Get().Any(x => x.UserName == user.UserName && x.UserPassword == user.UserPassword);
            if (!isValid)
            {
                var response = userdata.Create(user);
                return RedirectToAction("Login");
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
                    var data = (from us in context.Users
                                where us.UserName == name && us.UserPassword == password
                                select us).ToList();
                    Session["Id"] = data[0].UserId;
                    FormsAuthentication.SetAuthCookie(name, false);
                    if (role.IsUserInRole(name,"Admin"))
                    {
                        return RedirectToAction("Get", "User");
                    }
                    else
                    {
                        return RedirectToAction("GetBooks", "Book");
                    }
                }
                ModelState.AddModelError("", "Invalid username or password");
                
                return View();
            }
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}