//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ClinicManagementSystem.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    public partial class EducationFeedback
    {
        public int FeedbackID { get; set; }
        [DisplayName("User Name")]
        public string Username { get; set; }
        public Nullable<int> EducationID { get; set; }
        public string Content { get; set; }
        [DisplayName("Created Date")]
        public Nullable<System.DateTime> CreatedDate { get; set; }
    
        public virtual Customer Customer { get; set; }
        public virtual Education Education { get; set; }
    }
}
