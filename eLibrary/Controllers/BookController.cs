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
                var result = (bookdata.Get().Where(m => m.Quantity > 0)).Take(5);
                Session["Pagination"] = 5;
                return View(result);
            }
            else if (next != null)
            {
                int val = Convert.ToInt32(Session["Pagination"]);
                var result = (bookdata.Get().Where(m => m.Quantity > 0)).Skip(val).Take(5);
                if (!result.Any()) 
                {
                    result = (bookdata.Get().Where(m => m.Quantity > 0)).Take(5);
                    Session["Pagination"] = 5;
                }
                Session["Pagination"] = 5 + val;
                return View(result);
               
            }
            else
            {
                int val = Convert.ToInt32(Session["Pagination"]) -10;
                var result = (bookdata.Get().Where(m => m.Quantity > 0)).Skip(val).Take(5);
                if (!result.Any())
                {
                    result = (bookdata.Get().Where(m => m.Quantity > 0)).Take(5);
                    Session["Pagination"] = 5;
                }
                Session["Pagination"] = 5 + val;
                return View(result);
            }
            
       
        }


        public ActionResult GetBookByName(string BookName) 
        {
            var result = bookdata.Get().Where(x => x.BookName.ToLower() == BookName.ToLower());
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

        public ActionResult Create() 
        {
            return View();
        }

        [HttpPost]

        public ActionResult Create(BookDetail bookdetail) 
        {
            bookdata.Create(bookdetail);
            return RedirectToAction("GetBooks");
        }

        public ActionResult Delete(int id) 
        {
            bookdata.delete(id);
            return RedirectToAction("GetBooks");
        }
    }
}