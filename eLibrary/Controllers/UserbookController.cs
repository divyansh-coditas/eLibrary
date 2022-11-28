using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eLibrary.Services;

namespace eLibrary.Controllers
{
    public class UserbookController : Controller
    {
        BookDataAccess bookdata = new BookDataAccess();
        UserBookDataAccess userbooks = new UserBookDataAccess();
        UserDataAccess users = new UserDataAccess();
        public ActionResult GetAccess(int id)
        {
            var bookdetail = bookdata.Get(id);
            int Userid = Convert.ToInt32(Session["Id"]);
            var userdetail = users.Get(Userid);
            var isvalid = userbooks.Get().Any(m => m.UserId == userdetail.UserId && m.Bookname == bookdetail.BookName);
            if (isvalid || (bookdetail.Rating == "Adult" && userdetail.Age <= 18))
            {
                return View();
            }
            userbooks.Access(id, Userid);

            return RedirectToAction("GetById");

        }

        public ActionResult GetById() 
        {
            UserBookDataAccess userbook = new UserBookDataAccess();
            int Userid = Convert.ToInt32(Session["Id"]);
            var result = userbook.Get().Where(m => m.UserId == Userid && m.Is_Submitted == null);
            return View(result);
        }

        public ActionResult Submit(int id, bool? is_paid) 
        {
            var result = userbooks.Get().Where(m => m.BookId == id).FirstOrDefault();
            if (DateTime.Now > result.SubmissionDate)
            {
                TimeSpan time = DateTime.Now - result.SubmissionDate;
                int money = Convert.ToInt32(time.TotalDays * 15);
                if (money > 0 && is_paid == null)
                {
                    ViewBag.Money = money;
                    return View("View");
                }
            }
            bookdata.Submit(id);
            userbooks.Update(id, 0);
            return RedirectToAction("GetBooks","Book");
        }

    }
}