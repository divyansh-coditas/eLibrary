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
            var result = context.BookDetails.ToList();
            return result;
        }

        public IEnumerable<BookDetail> GetBooks(string name) 
        {
            //var data = context.BookDetails.ToList();
            var result = from book in context.BookDetails
                         join bookcat in context.Bookcategories on book.CategoryId equals bookcat.CategoryId
                         where bookcat.CategoryName == name
                         select book;
            return result;
        }
    }
}