using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eLibrary.Models
{
    public class SubscribedUser
    {
       // public int SubscriptionId { get; set; }
        public int UserId { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }

        public virtual User User { get; set; }
    }
}