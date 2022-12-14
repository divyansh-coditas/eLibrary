using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eLibrary.Controllers
{
    public class HomeController : Controller
    {
        // this is the home page
        public ActionResult Index()
        {
            return View();
        }

        // this is the about page
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}