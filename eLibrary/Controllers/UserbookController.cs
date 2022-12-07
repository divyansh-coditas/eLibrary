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
        BookDataAccess bookdata;
        UserBookDataAccess userbooks;
        UserDataAccess users;
        SubscrptionDataAccess subscribeduser;

        public UserbookController(BookDataAccess bookdata, UserBookDataAccess userbooks, UserDataAccess users, SubscrptionDataAccess subscribeduser) 
        {
            this.bookdata = bookdata;
            this.userbooks = userbooks; 
            this.users = users;
            this.subscribeduser = subscribeduser;
        }
 
        public ActionResult GetAccess(int id)
        {
            if (Session["Id"] != null)
            {
                var bookdetail = bookdata.Get(id);
                int Userid = Convert.ToInt32(Session["Id"]);
                var userdetail = users.Get(Userid);
                // checking whether the user have already this book or not
                var isvalid = userbooks.Get().Any(m => m.UserId == userdetail.UserId && m.Bookname == bookdetail.BookName && (m.Is_Paid == false || m.Is_Paid == null));
                if (isvalid)
                {
                    // if the user already have access of that book 
                    // then user will be Redirected to this View 
                    ViewBag.Message = "Sorry!!! You already have access of this book";
                    return View();
                }
                if (bookdetail.Rating == "Adult" && userdetail.Age <= 18)
                {
                    //user age is less than 18 and they are trying to access Adult books
                    ViewBag.Message = "Sorry!!! You are not authorised since your age is less than 18";
                    return View();

                }
                // if not then user will get access of that book
                userbooks.Access(id, Userid);
                TempData["Access"] = 1;
                return RedirectToAction("GetBooks", "Book");
            }
            else 
            {
                TempData["failed"] = 1;
                return RedirectToAction("Login", "User");      
            }

        }
        // this method will return the users what all books they have currently
        public ActionResult GetById() 
        {
            if (Session["Id"] != null)
            {
                int Userid = Convert.ToInt32(Session["Id"]);
                var result = userbooks.Get().Where(m => m.UserId == Userid && m.Is_Paid == null && m.Is_Approved == true);
                return View(result);
            }
            else 
            {
                TempData["failed"] = 1;
                return RedirectToAction("Login","User");
            }
        }

        //Book Submission
        public ActionResult Submit(int bookid, string str) 
        {
            if (Session["Id"] != null)
            {
                int userid = Convert.ToInt32(Session["Id"]);
                var result = userbooks.Get().Where(m => m.BookId == bookid && m.UserId == userid && (m.Is_Submitted == false || m.Is_Submitted == null)).FirstOrDefault();
                // ckeck whether the susubmission data is less than today's date or not
                if (DateTime.Now > result.SubmissionDate)
                {
                    TimeSpan? time = DateTime.Now - result.SubmissionDate;
                    // Total days multiplies 15
                    int money = Convert.ToInt32(time.Value.TotalDays * 15);
                    if (money > 0 && str == null)
                    {
                        // this will redirect to payment page
                        ViewBag.Money = money;
                        return View("Fine");
                    }
                    else
                    {
                        // after the payment the bookdetails table and userbooks table will get updated
                        bookdata.Submit(bookid);
                        userbooks.Update(bookid, money, userid);
                        TempData["ispaid"] = 1;
                        return RedirectToAction("GetBooks", "Book");
                    }
                }
                bookdata.Submit(bookid);
                userbooks.Update(bookid, 0, userid);
                TempData["ispaid"] = 1;
                return RedirectToAction("GetBooks", "Book");
            }
            else 
            {
                TempData["failed"] = 1;
                return RedirectToAction("Loign", "User");
            }
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
            TempData["approval"] = 1;
            return RedirectToAction("NotApprovedBooks");
        }

        [Authorize (Roles = "Admin")]
        public ActionResult DeleteUserBook(int detailid) 
        {
            // Deleting the userbookdetails if the user didn't approve particular book for the user
            userbooks.DeleteUserBook(detailid);
            TempData["rejected"] = 1;
            return RedirectToAction("NotApprovedBooks");
        }

    }
}