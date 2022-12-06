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
        BookDataAccess bookdata;
        CategoryDataAccess categorydata;

        public BookController() 
        {
            bookdata = new BookDataAccess();
            categorydata = new CategoryDataAccess();
        }
        
        // this action method will return all the books to the user ands also implemented pagination
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
                else
                {
                    Session["Pagination"] = 5 + val;
                }
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
                else
                {
                    Session["Pagination"] = 5 + val;
                }
                return View(result);
            }
            
       
        }

        // this action method will return the user book based on their search
        public ActionResult GetBookByName(string searchtext) 
        {
            var result = bookdata.Get().Where(x => x.BookName.ToLower().Contains(searchtext.ToLower()) || x.BookLanguage.ToLower() == searchtext.ToLower() 
                         || x.AuthorName.ToLower().Contains(searchtext.ToLower()) || x.Rating.ToLower() == searchtext.ToLower());
            if (result != null) 
            {
                return View(result);
            }
            return View();
        }


        // this action method is for editing the details of the book

        [Authorize(Roles = "Admin")]
        public ActionResult Edit() 
        {
            BookDetail book = new BookDetail();
            var categories = categorydata.Get().ToList();
            var categoryname = from cat in categories
                               select cat.CategoryName;
            ViewBag.CategoryName = categoryname;
            return View(book);
        }

        [HttpPost]
        public ActionResult Edit(int id, BookDetail bookDetail, string categoryname) 
        {
            var categoryid = categorydata.Get().Where(m => m.CategoryName == categoryname).Select(m => m.CategoryId).FirstOrDefault();
            bookDetail.CategoryId = categoryid;
            bookdata.Edit(id, bookDetail);
            return RedirectToAction("GetBooks");
        }

        // this action method is for creating a new book in the databse

        [Authorize(Roles = "Admin")]
        public ActionResult Create() 
        {
            var categories = categorydata.Get().ToList();
            var categoryname = from cat in categories
                               select cat.CategoryName;
            ViewBag.CategoryName = categoryname;
            return View();
        }

        [HttpPost]
        public ActionResult Create(BookDetail bookdetail, string categoryname) 
        {
            var categoryid = categorydata.Get().Where(m => m.CategoryName == categoryname).Select(m => m.CategoryId).FirstOrDefault();
            bookdetail.CategoryId = categoryid;
            bookdata.Create(bookdetail);
            return RedirectToAction("GetBooks");
        }

        // this action method is for deletion of the existing book

        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id) 
        {
            try
            {
                bookdata.delete(id);
                return RedirectToAction("GetBooks");
            }
            catch (Exception)
            {

                return View("Error");
            }
        }
    }
}