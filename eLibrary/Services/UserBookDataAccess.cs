using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using eLibrary;
using eLibrary.Models;

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

        public IEnumerable<UserBookDetail1> Access(int id, int val) 
        {
            BookDataAccess bookdata = new BookDataAccess();
            UserBookDataAccess user = new UserBookDataAccess();
            UserDataAccess users = new UserDataAccess();
            bookdata.Update(id);
            UserBookDetail userbookdetail1 = new UserBookDetail()
            {
                UserId = val,
                BookId = id,
                IssueDate = DateTime.Now,
                SubmissionDate = DateTime.Now.AddDays(7),
            };
            user.Create(userbookdetail1);
            List<UserBookDetail1> items = new List<UserBookDetail1>();
            var result1 = user.Get();
            foreach (var item in result1)
            {
                if (item.UserId == val)
                {
                    items.Add(new UserBookDetail1
                    {
                        UserId = item.UserId,
                        BookId = item.BookId,
                        IssueDate = item.IssueDate.ToShortDateString(),
                        SubmissionDate = item.SubmissionDate.ToShortDateString()
                    }); ;
                }
            }

            return items;
        }
    }
}