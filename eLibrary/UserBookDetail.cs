//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace eLibrary
{
    using System;
    using System.Collections.Generic;
    
    public partial class UserBookDetail
    {
        public int UserId { get; set; }
        public int BookId { get; set; }
        public Nullable<System.DateTime> SubmittedOn { get; set; }
        public Nullable<int> Fine { get; set; }
        public Nullable<bool> Is_Submitted { get; set; }
        public Nullable<bool> Is_Paid { get; set; }
        public int DetailId { get; set; }
        public string Bookname { get; set; }
        public Nullable<bool> Is_Approved { get; set; }
        public Nullable<System.DateTime> SubmissionDate { get; set; }
        public Nullable<System.DateTime> IssueDate { get; set; }
    
        public virtual BookDetail BookDetail { get; set; }
        public virtual User User { get; set; }
    }
}
