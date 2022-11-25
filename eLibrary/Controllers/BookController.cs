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
        //GET: Book
        BookDataAccess bookdata = new BookDataAccess();

        public ActionResult GetBooks()
        {
            var result = bookdata.Get();
            return View(result);
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
    }
}