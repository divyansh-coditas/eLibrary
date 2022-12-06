using System;
using System.Collections.Generic;
using System.Linq;
using eLibrary;


namespace eLibrary.Services
{
    public class CategoryDataAccess
    {
        eLibraryEntities context = new eLibraryEntities();

        // this method will return all the categories of the books
        public IEnumerable<Bookcategory> Get()
        {
            var result = context.Bookcategories.ToList();
            return result;
        }

        // this method is for creation of the books
        public void create(Bookcategory bookcategory) 
        {
            var res = context.Bookcategories.Add(bookcategory);
            context.SaveChanges();
        }

        // this method is for updating already existing book category
        public void Update(int id, Bookcategory bookcategory)
        {
            var category = context.Bookcategories.Find(id);
            category.CategoryName = bookcategory.CategoryName;
            context.SaveChanges();          
        }

        // this method is to delete the book category
        public void Delete(int id) 
        {          
                var category = context.Bookcategories.Find(id);
                context.Bookcategories.Remove(category);
                context.SaveChanges();          
        }

    }
}