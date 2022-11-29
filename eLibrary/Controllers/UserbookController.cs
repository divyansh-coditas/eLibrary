using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eLibrary.Services;

namespace eLibrary.Controllers
{
    [Authorize]
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
            var isvalid = userbooks.Get().Any(m => m.UserId == userdetail.UserId && m.Bookname == bookdetail.BookName && (m.Is_Paid == false || m.Is_Paid == null));
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
            var result = userbook.Get().Where(m => m.UserId == Userid && (m.Is_Paid == false || m.Is_Paid == null));
            return View(result);
        }

        public ActionResult Submit(int id, string str) 
        {
            int userid = Convert.ToInt32(Session["Id"]);
            var result = userbooks.Get().Where(m => m.BookId == id && m.UserId == userid && (m.Is_Submitted == false || m.Is_Submitted == null)).FirstOrDefault();
            if (DateTime.Now > result.SubmissionDate)
            {
                TimeSpan time = DateTime.Now - result.SubmissionDate;
                int money = Convert.ToInt32(time.TotalDays * 15);
                if (money > 0 && id != 0 && str == null)
                {
                    ViewBag.Money = money;
                    return View("View");
                }
                else 
                {
                    bookdata.Submit(id);
                    userbooks.Update(id, money, userid);
                    return RedirectToAction("GetBooks", "Book");
                }
            }
            bookdata.Submit(id);
            userbooks.Update(id, 0, userid);
            return RedirectToAction("GetBooks","Book");
        }

        public ActionResult GetAllUsersWithBooks() 
        {
            var result = userbooks.Get();
            return View(result);
        }

    }
}