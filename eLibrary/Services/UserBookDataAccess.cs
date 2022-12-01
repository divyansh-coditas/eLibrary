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
        BookDataAccess bookdata = new BookDataAccess();
        SubscrptionDataAccess subscribe = new SubscrptionDataAccess();
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
            var bookDetail = bookdata.Get(bookid);           
            UserBookDetail userbookdetail = new UserBookDetail()
            {
                UserId = userid,
                BookId = bookid,
                Bookname = bookDetail.BookName,
                IssueDate = DateTime.Now,
            };
            Create(userbookdetail);
            return userbookdetail;
        }

        public UserBookDetail Update(int id, int fine, int userid)
        {
            var result = context.UserBookDetails.ToList().Where(m => m.BookId == id && m.UserId == userid && (m.Is_Submitted == false || m.Is_Submitted == null)).FirstOrDefault();
            result.SubmittedOn = DateTime.Now;
            result.Is_Submitted = true;
            result.Fine = fine;
            result.Is_Paid = true;
            context.SaveChanges();
            return result;
        }

        public IEnumerable<sp_GetAllUsersBooks_Result> GetAllUsersHavingBooks()
        {
            var result = context.sp_GetAllUsersBooks().ToList().Where(m => m.SubmittedOn == null);
            return result;
        }

        public IEnumerable<sp_GetNotApprovedUserBooks_Result> GetNotApprovedUserBooks()
        {
            var result = context.sp_GetNotApprovedUserBooks().ToList();
            return result;
        }

        //Update UserBookDetails After Approval
        public UserBookDetail UpdateUserBook(int id,UserBookDetail userBookDetail) 
        {
            var result = context.UserBookDetails.Find(id);
            result.IssueDate = userBookDetail.IssueDate;
            result.SubmissionDate = userBookDetail.SubmissionDate;
            result.Is_Approved = userBookDetail.Is_Approved;

            context.SaveChanges();
            return result;
        }

        // Delete UserBookDetails for Approval

        public UserBookDetail DeleteUserBook(int detailid) 
        {
            var result = context.UserBookDetails.Find(detailid);
            context.UserBookDetails.Remove(result);
            context.SaveChanges();
            return result;
        }
    }
}