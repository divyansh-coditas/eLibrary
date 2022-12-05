using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eLibrary.Services
{
    public class UserDataAccess
    {
        eLibraryEntities context = new eLibraryEntities();

        // this method will return all the userdetails of the application
        public IEnumerable<User> Get()
        {
            var result = context.Users.ToList();
            return result;
        }

        // this method is to create new user
        public int Create(User user)
        {
            context.Users.Add(user);
            context.SaveChanges();
            return user.UserId;
        }

        // this method is to search user based on their id
        public User Get(int id) 
        {
            var result = context.Users.Find(id);
            return result;
        }
    }

}