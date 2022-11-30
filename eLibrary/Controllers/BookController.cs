using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eLibrary.Services;

namespace eLibrary.Controllers
{
    [Authorize]
    public class BookController : Controller
    {
        BookDataAccess bookdata = new BookDataAccess();
       
        public ActionResult GetBooks(string next, string previous)
        {
            if (next == null && previous == null)
            {
                var result = (bookdata.Get().Where(m => m.Quantity > 0)).Take(6);
                Session["Pagination"] = 6;
                return View(result);
            }
            else if (next != null)
            {
                int val = Convert.ToInt32(Session["Pagination"]);
                var result = (bookdata.Get().Where(m => m.Quantity > 0)).Skip(val).Take(6);
                if (!result.Any()) 
                {
                    result = (bookdata.Get().Where(m => m.Quantity > 0)).Take(6);
                    Session["Pagination"] = 6;
                }
                Session["Pagination"] = 6 + val;
                return View(result);
               
            }
            else
            {
                int val = Convert.ToInt32(Session["Pagination"]) -12;
                var result = (bookdata.Get().Where(m => m.Quantity > 0)).Skip(val).Take(6);
                if (!result.Any())
                {
                    result = (bookdata.Get().Where(m => m.Quantity > 0)).Take(6);
                    Session["Pagination"] = 6;
                }
                Session["Pagination"] = 6 + val;
                return View(result);
            }
            
       
        }

        public ActionResult Get(string next, string previous) 
        {
            if (next == null && previous == null)
            {
                var result = (bookdata.Get()).Take(6);
                Session["Pagination"] = 6;
                return View("GetBooks",result);
            }
            else if (next != null)
            {
                int val = Convert.ToInt32(Session["Pagination"]);
                var result = (bookdata.Get().Skip(val)).Take(6);
                if (!result.Any())
                {
                    result = (bookdata.Get()).Take(6);
                    Session["Pagination"] = 6;
                }
                Session["Pagination"] = 6 + val;
                return View("GetBooks", result);

            }
            else
            {
                int val = Convert.ToInt32(Session["Pagination"]) - 12;
                var result = (bookdata.Get()).Skip(val).Take(6);
                if (!result.Any())
                {
                    result = (bookdata.Get()).Take(6);
                    Session["Pagination"] = 6;
                }
                Session["Pagination"] = 6 + val;
                return View("GetBooks", result);
            }
        }

        public ActionResult GetBookByName(string BookName) 
        {
            var result = bookdata.Get().Where(x => x.BookName == BookName);
            if (result != null) 
            {
                return View(result);
            }
            return View();
        }

        public ActionResult GetAccess(int id) 
        {
            return RedirectToAction("GetAccess");
        }

        public ActionResult Edit() 
        {
            return View();
        }

        [HttpPost]

        public ActionResult Edit(int id, BookDetail bookDetail) 
        {
            bookdata.Edit(id, bookDetail);
            return RedirectToAction("GetBooks");
        }
    }
}