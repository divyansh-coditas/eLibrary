using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eLibrary.Services;

namespace eLibrary.Controllers
{
    public class BookController : Controller
    {
        //GET: Book
        BookDataAccess bookdata = new BookDataAccess();
        eLibraryEntities context = new eLibraryEntities();

        public ActionResult GetBooks()
        {
            return View();
        }


        [HttpPost]
        public ActionResult GetBooks(string bookcategory)
        {
            Bookcategory bookcategories = null;
            List<BookDetail> bookDetails = null;
            Tuple<Bookcategory, List < BookDetail >> tuple = null;
            string name = bookcategory;
            if (name != null)
            {
                var result = (bookdata.GetBooks(name)).ToList();
                var data = context.Bookcategories.ToList();
                var result1 = from cat in data
                              where cat.CategoryName == name
                              select cat;
                Bookcategory bookcategory1 = result1.FirstOrDefault();
                tuple = new Tuple<Bookcategory, List<BookDetail>>(bookcategory1, result);
                return View(tuple);
            }
            return View();
        }

        [HttpPost]
        public ActionResult Details(string bookcategory) 
        {
            return RedirectToAction("GetBooks", new { bookcategory = bookcategory});
            
        }
    }
}