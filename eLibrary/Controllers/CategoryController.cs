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
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Get() 
        {
            var result = catdata.Get();
            return View(result);
        }

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

        
        public ActionResult Delete(int id) 
        {
            try
            {
                var result = catdata.Delete(id);
                return RedirectToAction("Get");
            }
            catch (Exception ex) 
            {
                return View("Error");
            }
        }

        
        public ActionResult GetBooks(string CategoryName)
        {
            //string bookname = book.CategoryName;
            var result = bookdata.GetBooks(CategoryName);
            return View(result);
        }
    }
}