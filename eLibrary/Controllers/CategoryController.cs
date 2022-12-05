using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eLibrary.Services;

namespace eLibrary.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        CategoryDataAccess catdata = new CategoryDataAccess();
        BookDataAccess bookdata = new BookDataAccess();

        // this action method will return all the categories to the user
        public ActionResult Get() 
        {
            var result = catdata.Get();
            return View(result);
        }

        // this action method is for creating new books 

        [Authorize(Roles = "Admin")]
        public ActionResult Create() 
        {
            return View();
        }

        [HttpPost]

        public ActionResult Create(Bookcategory bookcategory) 
        {
            var result = catdata.create(bookcategory);
            return RedirectToAction("Get","Category");
        }

        // this action method for editing the details of the existing book 

        [Authorize(Roles = "Admin")]
        public ActionResult Edit() 
        {
            return View();
        }

        [HttpPost]

        public ActionResult Edit(int id, Bookcategory bookcategory) 
        { 
            var result = catdata.Update(id, bookcategory);
            return RedirectToAction("Get");
        }

        // this action method is for deleting the existing from the record 
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id) 
        {
            try
            {
                var result = catdata.Delete(id);
                return RedirectToAction("Get");
            }
            catch (Exception) 
            {
                return View("Error");
            }
        }

        // this action method will return all the books under the category provided by the user
        public ActionResult GetBooks(string CategoryName)
        {
            //string bookname = book.CategoryName;
            var result = bookdata.GetBooks(CategoryName);
            return View(result);
        }
    }
}