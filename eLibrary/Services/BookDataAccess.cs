using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eLibrary.Services
{
    public class BookDataAccess
    {
        eLibraryEntities context = new eLibraryEntities();

        // this will return all the bookdetails
        public IEnumerable<BookDetail> Get() 
        {
            var result = context.BookDetails.ToList().Where(m => m.Quantity > 0);
            return result;
        }
        
        // this will retturn the books based on the category name
        public IEnumerable<BookDetail> GetBooks(string name) 
        {
            var result = from book in context.BookDetails
                         join bookcat in context.Bookcategories on book.CategoryId equals bookcat.CategoryId
                         where bookcat.CategoryName == name
                         select book;
            //var result = context.BookDetails.Join(context.Bookcategories , m => m.CategoryId, n => n.CategoryId, (m,n) 
            //             => new { m, n}).Where(x => x.n.CategoryName == name).Select(y => new { y.m }));

            return result;
        }
        
        // this method will return the book based on bookid
        public BookDetail Get(int id) 
        {
            var result = context.BookDetails.Find(id);
            return result;
        }

        // this method is to update the book details
        public void Update(int id) 
        {
            var book = context.BookDetails.Find(id);
            book.Quantity -= 1;
            context.SaveChanges();
        }

        // this method is to incearse the quanitiy of the books when user will submit it
        public void Submit(int id) 
        {
            var book = context.BookDetails.Find(id);
            book.Quantity += 1;
            context.SaveChanges();
        }

        // this method is edit the details of the book 
        public void Edit(int id, BookDetail bookdetail) 
        {
            var book = context.BookDetails.Find(id);
            book.BookName = bookdetail.BookName;
            book.AuthorName = bookdetail.AuthorName;
            book.RackNo = bookdetail.RackNo;
            book.Rating = bookdetail.Rating;
            book.Quantity = bookdetail.Quantity;

            context.SaveChanges();
           
        }

        // this method is for creating new books
        public void Create( BookDetail bookdetail) 
        {
            context.BookDetails.Add(bookdetail);
            context.SaveChanges();
        }

        // this method is for deleting the book
        public void delete(int id) 
        {
            var result = context.BookDetails.Find(id);
            context.BookDetails.Remove(result);
            context.SaveChanges();
        }
    }
}