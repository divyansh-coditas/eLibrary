using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using eLibrary.Services;
using System.Web.Security;

namespace eLibrary.Services
{
    public class SubscrptionDataAccess
    {
        eLibraryEntities context = new eLibraryEntities();

        // this method will return all the subscribed user
        public IEnumerable<Subscription> Get() 
        {
            var result = context.Subscriptions.ToList();
            return result;
        }

        // this method is for subscription of the user
        public Subscription Create(Subscription subscription) 
        {
            var result = context.Subscriptions.Add(subscription);
            context.SaveChanges();
            return result;        
        }

        // this methos will return all subscribed user to tha admin
        public IEnumerable<sp_getAllSubscribedUser_Result> GetAllSubscribedUser() 
        {
            var result = context.sp_getAllSubscribedUser().ToList();
            return result;
        }
    }
}