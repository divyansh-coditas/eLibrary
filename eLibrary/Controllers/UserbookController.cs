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
        SubscrptionDataAccess subscribeduser = new SubscrptionDataAccess();
        public ActionResult GetAccess(int id)
        {
            var bookdetail = bookdata.Get(id);
            int Userid = Convert.ToInt32(Session["Id"]);
            var userdetail = users.Get(Userid);
            // checking whether the user have already this book or not
            var isvalid = userbooks.Get().Any(m => m.UserId == userdetail.UserId && m.Bookname == bookdetail.BookName && (m.Is_Paid == false || m.Is_Paid == null));
            if (isvalid || (bookdetail.Rating == "Adult" && userdetail.Age <= 18))
            {
                // if the user already have access of that book or user age is less than 18 and they are trying to access Adult books
                // then user will be Redirected to this View 
                return View();
            }
            // if not then user will get access of that book
            userbooks.Access(id, Userid);

            return RedirectToAction("GetBooks", "Book");

        }

        public ActionResult GetById() 
        { 
            int Userid = Convert.ToInt32(Session["Id"]);
            var result = userbooks.Get().Where(m => m.UserId == Userid && m.Is_Paid == null && m.Is_Approved == true);
            return View(result);
        }

        //Book Submission
        public ActionResult Submit(int id, string str) 
        {
            int userid = Convert.ToInt32(Session["Id"]);
            var result = userbooks.Get().Where(m => m.BookId == id && m.UserId == userid && (m.Is_Submitted == false || m.Is_Submitted == null)).FirstOrDefault();
            // ckeck whether the susubmission data is less than today's date or not
            if (DateTime.Now > result.SubmissionDate)
            {
                TimeSpan? time = DateTime.Now - result.SubmissionDate;
                // Total days multiplies 14
                int money = Convert.ToInt32(time.Value.TotalDays * 15);
                if (money > 0 && id != 0 && str == null)
                {
                    // this will resirect to payment page
                    ViewBag.Money = money;
                    return View("View");
                }
                else 
                {
                    // after the payment the bookdetails table and userbooks table will get updated
                    bookdata.Submit(id);
                    userbooks.Update(id, money, userid);
                    return RedirectToAction("GetBooks", "Book");
                }
            }
            bookdata.Submit(id);
            userbooks.Update(id, 0, userid);
            return RedirectToAction("GetBooks","Book");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult GetAllUsersWithBooks() 
        {
            // this will return currently what all users have which books
            var result = userbooks.GetAllUsersHavingBooks();
            return View(result);
        }

        //Only Admin can access this method as admin can only approve Books requested by user.

        [Authorize(Roles = "Admin")]
        public ActionResult NotApprovedBooks() 
        {
            // this will return books requested by user for access so that Admin can approve
            var result = userbooks.GetNotApprovedUserBooks();
            return View(result);
        }

        [Authorize(Roles = "Admin")]

        public ActionResult Approve(int detailid) 
        {
            DateTime date;
            var result = userbooks.Get().Where(m => m.DetailId == detailid).FirstOrDefault();
            var isSubscribedUser = subscribeduser.Get().Any(m => m.UserId == result.UserId);
            if (isSubscribedUser)
            {
                // if the user has already subscribed then the user will get access of that book for 14 days
                date = DateTime.Now.AddDays(14);
            }
            else
            {
                //if not thet user will acccess for 7 days
                date = DateTime.Now.AddDays(7);
            }


            UserBookDetail userBookDetail = new UserBookDetail()
            {
                IssueDate = DateTime.Now,
                SubmissionDate = date,
                Is_Approved = true
            };
            // Updating the Bookdata 
            bookdata.Update(result.BookId);
            // updating the userbookdata
            userbooks.UpdateUserBook(detailid, userBookDetail);
            return RedirectToAction("NotApprovedBooks");
        }

        [Authorize (Roles = "Admin")]
        public ActionResult DeleteUserBook(int detailid) 
        {
            // Deleting the userbookdetails if the user didn't approve particular book for the user
            userbooks.DeleteUserBook(detailid);  
            return RedirectToAction("NotApprovedBooks");
        }

    }
}