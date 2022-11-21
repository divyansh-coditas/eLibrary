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
        // GET: Category
        CategoryDataAccess catdata = new CategoryDataAccess();
        public ActionResult Index()
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
            return View(result);
        }

        public ActionResult Edit() 
        {
            return View();
        }

        [HttpPost]

        public ActionResult Edit(int id, Bookcategory bookcategory) 
        { 
            var result = catdata.Update(id, bookcategory);
            return RedirectToAction("Index");
        }

        
        public ActionResult Delete(int id) 
        {
            var result = catdata.Delete(id);
            return RedirectToAction("Index");
        }
    }
}