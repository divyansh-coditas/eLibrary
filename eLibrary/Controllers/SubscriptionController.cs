using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eLibrary.Services;
using eLibrary.Models; 

namespace eLibrary.Controllers
{
    public class SubscriptionController : Controller
    {
        SubscrptionDataAccess data = new SubscrptionDataAccess();

        // GET: Subscription
        public ActionResult Get()
        {
            var users = data.Get();
            List<SubscribedUser> result = new List<SubscribedUser>();
            foreach (var item in users) 
            {
                result.Add(new SubscribedUser { UserId = item.UserId, StartDate = item.StartDate.ToShortDateString(), 
                    EndDate = item.EndDate.ToShortDateString()});
            }
            return View(result);
        }

        public ActionResult Create() 
        {
            var result = data.Get();
            foreach (var v in result) 
            {
                if (v.UserId == Convert.ToInt32(TempData["Id"])) 
                {
                    List<Subscription> li = new List<Subscription>();
                    li.Add(new Subscription { EndDate = v.EndDate });
                    ViewBag.Message = li[0].EndDate;
                    return View("View", ViewBag.Message);
                }
            }
            return View();
        }

        [HttpPost]
        public ActionResult Create(string str, string month) 
        {
            var result = data.Get();
            foreach (var v in result)
            {
                if (v.UserId == Convert.ToInt32(TempData["Id"]))
                {
                    List<Subscription> li = new List<Subscription>();
                    li.Add(new Subscription { EndDate = v.EndDate });
                    ViewBag.Message = li[0].EndDate;
                    return View("View", ViewBag.Message);
                }
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
                    UserId = Convert.ToInt32(TempData["Id"]),
                    StartDate = DateTime.Today.Date,
                    EndDate = date
                };
                
            
                return RedirectToAction("GetBooks","Book");
            }


            
        }
    }
}