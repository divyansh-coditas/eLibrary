using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eLibrary.Models;
using eLibrary.Services;

namespace eLibrary.Controllers
{
    public class UserbookController : Controller
    {   
        // GET: Userbook
        public ActionResult GetAccess(int id)
        {
            BookDataAccess bookdata = new BookDataAccess();
            UserBookDataAccess user = new UserBookDataAccess();
            UserDataAccess users = new UserDataAccess();
          
            var result = bookdata.Get(id);
            
            int value = Convert.ToInt32(TempData["Id"]);
            var result1 = users.Get(value);
            if (result.Rating == "Adult" && result1.Age >= 18)
            {
                var output = user.Access(id, value);
                return View(output);
            }
            else if(result.Rating == "Adult" && result1.Age <= 18)
            {
                return View();
            }

            var output1 = user.Access(id, value);

            return View(output1);

        }
    }
}