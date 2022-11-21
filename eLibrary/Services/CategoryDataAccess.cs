using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;

namespace eLibrary.Services
{
    [Authorize]
    public class CategoryDataAccess
    {
        eLibraryEntities context = new eLibraryEntities();

        public IEnumerable<Bookcategory> Get()
        {
            var result = context.Bookcategories.ToList();
            return result;
        }
    }
}