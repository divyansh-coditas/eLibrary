using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using eLibrary;

namespace eLibrary.Services
{
    public class UserBookDataAccess
    {
        eLibraryEntities context = new eLibraryEntities();
        public IEnumerable<UserBookDetail> Get() 
        {
            var result = context.UserBookDetails.ToList(); 
            return result;
        }

        public int Create(UserBookDetail userBookDetail) 
        {
            context.UserBookDetails.Add(userBookDetail);
            context.SaveChanges();
            return userBookDetail.UserId;
        }

        public UserBookDetail Access(int bookid, int userid) 
        {
            BookDataAccess bookdata = new BookDataAccess();
            UserBookDataAccess userbooks = new UserBookDataAccess();
            UserDataAccess users = new UserDataAccess();
            SubscrptionDataAccess subscribe = new SubscrptionDataAccess();
            DateTime date;
            var bookDetail = bookdata.Get(bookid);
            bookdata.Update(bookid);
            var isvalid = subscribe.Get().Any(m => m.UserId == userid);
            if (isvalid) 
            {
                date = DateTime.Now.AddDays(14);
            }
            else
            {
                date = DateTime.Now.AddDays(7);
            }
            UserBookDetail userbookdetail = new UserBookDetail()
            {
                UserId = userid,
                BookId = bookid,
                Bookname = bookDetail.BookName,
                IssueDate = DateTime.Now,
                SubmissionDate = date,
            };
            userbooks.Create(userbookdetail);
            return userbookdetail;
        }

        public UserBookDetail Update(int id,int fine, int userid) 
        {
            var result = context.UserBookDetails.ToList().Where(m => m.BookId == id && m.UserId == userid && (m.Is_Submitted == false || m.Is_Submitted == null)).FirstOrDefault();
            result.SubmittedOn = DateTime.Now;
            result.Is_Submitted = true;
            result.Fine = fine;
            result.Is_Paid = true;
            context.SaveChanges();
            return result;
        }
    }
}