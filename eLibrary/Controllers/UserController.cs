﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using eLibrary.Services;
using System.Web.Security;

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
            user.RoleID = 2;
            bool isValid = userdata.Get().Any(x => x.UserName == user.UserName && x.C_Password == user.C_Password);
            if (!isValid)
            {
                var response = userdata.Create(user);
                return RedirectToAction("Create");
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
                bool isValid = context.Users.Any(u => u.UserName == name && u.C_Password == password);
                var data = (from us in context.Users
                            where us.UserName == name && us.C_Password == password
                            select us).ToList();
                TempData["Id"] = data[0].UserId;
                if (isValid)
                {
                    FormsAuthentication.SetAuthCookie(name, false);
                    if (data[0].RoleID == 1)
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