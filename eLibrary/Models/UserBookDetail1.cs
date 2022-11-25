using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eLibrary.Models
{
    public class UserBookDetail1
    {
        public int UserId { get; set; }
        public int BookId { get; set; }
        public string IssueDate { get; set; }
        public string SubmissionDate { get; set; }
        public string SubmittedOn { get; set; }
        public Nullable<int> Fine { get; set; }
        public Nullable<bool> Is_Submitted { get; set; }
        public Nullable<bool> Is_Paid { get; set; }
        public int DetailId { get; set; }
    }
}