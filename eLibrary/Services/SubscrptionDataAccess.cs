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

        public IEnumerable<Subscription> Get() 
        {
            var result = context.Subscriptions.ToList();
            return result;
        }

        public Subscription Create(Subscription subscription) 
        {
            var result = context.Subscriptions.Add(subscription);
            context.SaveChanges();
            return result;
            
        }
    }
}