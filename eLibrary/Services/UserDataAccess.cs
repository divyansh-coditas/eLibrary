using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eLibrary.Services
{
    public class UserDataAccess
    {
        eLibraryEntities context = new eLibraryEntities();

        public IEnumerable<User> Get()
        {
            var result = context.Users.ToList();
            return result;
        }

        public int Create(User user)
        {
            context.Users.Add(user);
            context.SaveChanges();
            return user.UserId;
        }
    }
}