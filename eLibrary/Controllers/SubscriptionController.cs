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
        public ActionResult Get()
        {
            var result = data.Get();
            return View(result);
        }

        public ActionResult Create() 
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(string str) 
        {
            int month = Convert.ToInt32(str);
            DateTime date = DateTime.Today.AddMonths(month).Date;
          
            Subscription subscription = new Subscription() { UserId = Convert.ToInt32(TempData["Id"]), StartDate = DateTime.Today.Date,
                EndDate = date.Date
            };


            data.Create(subscription);
            return RedirectToAction("Get");
        }
    }
}