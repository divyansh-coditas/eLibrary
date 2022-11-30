using System;
using System.Collections.Generic;
using System.Linq;
using eLibrary;


namespace eLibrary.Services
{
    public class CategoryDataAccess
    {
        eLibraryEntities context = new eLibraryEntities();

        public IEnumerable<Bookcategory> Get()
        {
            var result = context.Bookcategories.ToList();
            return result;
        }

        public Bookcategory create(Bookcategory bookcategory) 
        {
            var res = context.Bookcategories.Add(bookcategory);
            context.SaveChanges();
            return res;
        }

        public Bookcategory Update(int id, Bookcategory bookcategory)
        {
            var category = context.Bookcategories.Find(id);
            category.CategoryName = bookcategory.CategoryName;
            context.SaveChanges();
            return category;
            
        }

        public bool Delete(int id) 
        {
            
                var category = context.Bookcategories.Find(id);
                if (category == null)
                {
                    throw new Exception("Record to be deleted is not found");
                }
                context.Bookcategories.Remove(category);
                context.SaveChanges();
                return true;           
        }

    }
}