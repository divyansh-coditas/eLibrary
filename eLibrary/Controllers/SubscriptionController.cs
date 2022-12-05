using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eLibrary.Services; 

namespace eLibrary.Controllers
{
    [Authorize]
    public class SubscriptionController : Controller
    {
        SubscrptionDataAccess data = new SubscrptionDataAccess();
        eLibraryEntities context = new eLibraryEntities();

        // this method will return all the user with subscription
        
        [Authorize (Roles = "Admin")]
        public ActionResult Get()
        {
            var users = data.Get();
            return View(users);
        }

        //this is method is for users  for subscription

        [Authorize (Roles = "User")]
        public ActionResult Create() 
        {
            if (Session["Id"] != null)
            {
                var userexist = data.Get().Where(m => m.UserId == Convert.ToInt32(Session["Id"])).FirstOrDefault();
                // if the user has already subscribed then the user will be directed to other view 
                if (userexist != null)
                {
                    ViewBag.Message = userexist.EndDate.ToShortDateString();
                    return View("View");
                }
                // else the user be directed to subscription page
                return View();
            }
            else 
            {
                TempData["failed"] = 1;
                return RedirectToAction("Login", "User");
            }
        }

        [HttpPost]
        public ActionResult Create(string months, string money) 
        {
            if (Session["Id"] != null)
            {
                var result = data.Get().Where(m => m.UserId == Convert.ToInt32(Session["Id"])).FirstOrDefault();
                if (result != null)
                {
                    ViewBag.Message = result.EndDate;
                    return View("View");
                }

                // first user will select the months for subscripttion and then he will be redirected to the payment page

                if (months != null)
                {
                    ViewBag.Message = Convert.ToInt32(months) * 150;
                    return View(ViewBag.Message);
                }
                else
                {
                    // after paynment is done the user data will be added in the subscription table
                    int month = Convert.ToInt32(money) / 150;
                    DateTime date = DateTime.Today.AddMonths(month).Date;

                    Subscription subscription = new Subscription()
                    {
                        UserId = Convert.ToInt32(Session["Id"]),
                        StartDate = DateTime.Today.Date,
                        EndDate = date
                    };
                    data.Create(subscription);
                    TempData["success"] = 1;
                    return RedirectToAction("GetBooks", "Book");
                }
            }
            else 
            {
                TempData["failed"] = 1;
                return RedirectToAction("Login", "User");
            }
        }

        // this action method will return all the subscribed user 
        [Authorize(Roles = "Admin")]
        public ActionResult GetAllSubscribedUser() 
        {
            var result = data.GetAllSubscribedUser();
            return View(result);
        }
    }
}