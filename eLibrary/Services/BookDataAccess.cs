using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eLibrary.Services
{
    public class BookDataAccess
    {
        eLibraryEntities context = new eLibraryEntities();

        public IEnumerable<BookDetail> Get() 
        {
            var result = context.BookDetails.ToList().Where(m => m.Quantity > 0);
            return result;
        }

        public IEnumerable<BookDetail> GetBooks(string name) 
        {
            var result = from book in context.BookDetails
                         join bookcat in context.Bookcategories on book.CategoryId equals bookcat.CategoryId
                         where bookcat.CategoryName == name
                         select book;
            return result;
        }

        public BookDetail Get(int id) 
        {
            var result = context.BookDetails.Find(id);
            return result;
        }

        public BookDetail Update(int id) 
        {
            var book = context.BookDetails.Find(id);
            book.Quantity -= 1;
            context.SaveChanges();
            return book;
        }

        public BookDetail Submit(int id) 
        {
            var book = context.BookDetails.Find(id);
            book.Quantity += 1;
            context.SaveChanges();
            return book;
        }
    }
}