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
    }
}