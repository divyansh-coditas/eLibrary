using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eLibrary.Services; 

namespace eLibrary.Controllers
{
    public class SubscriptionController : Controller
    {
        SubscrptionDataAccess data = new SubscrptionDataAccess();

        // GET: Subscription
        [Authorize (Roles = "Admin")]
        public ActionResult Get()
        {
            var users = data.Get();
            return View(users);
        }

        [Authorize (Roles = "User")]
        public ActionResult Create() 
        {
            var result = data.Get().Where(m => m.UserId == Convert.ToInt32(Session["Id"])).FirstOrDefault();
            if (result != null)
            {
                ViewBag.Message = result.EndDate.ToShortDateString();
                return View("View");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Create(string str, string month) 
        {
            var result = data.Get().Where(m => m.UserId == Convert.ToInt32(Session["Id"])).FirstOrDefault();
            if (result != null)
            {
                ViewBag.Message = result.EndDate;
                return View("View");
            }

            if (str != null)
            {
                ViewBag.Message = int.Parse(str) * 150;
                return View(ViewBag.Message);
            }
            else
            {
                int month1 = Convert.ToInt32(month)/150;
                DateTime date = DateTime.Today.AddMonths(month1).Date;

                Subscription subscription = new Subscription()
                {
                    UserId = Convert.ToInt32(Session["Id"]),
                    StartDate = DateTime.Today.Date,
                    EndDate = date
                };
                data.Create(subscription);
                return RedirectToAction("GetBooks","Book");
            }   
        }

        public ActionResult GetAllSubscribedUser() 
        {
            var result = data.GetAllSubscribedUser();
            return View(result);
        }
    }
}