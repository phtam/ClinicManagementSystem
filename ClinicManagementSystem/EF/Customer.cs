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
    using System.ComponentModel.DataAnnotations;
    using System.Web;

    public partial class Customer
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Customer()
        {
            this.EducationFeedbacks = new HashSet<EducationFeedback>();
            this.MedicineFeedbacks = new HashSet<MedicineFeedback>();
            this.MedicineOrders = new HashSet<MedicineOrder>();
            this.ScientificApparatusOrders = new HashSet<ScientificApparatusOrder>();
            this.ScientificApparatusFeedbacks = new HashSet<ScientificApparatusFeedback>();
        }
    
        public string Username { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please select birthdate")]
        [DataType(DataType.Date, ErrorMessage = "Birthdate must be date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> DateOfBirth { get; set; }
        public Nullable<bool> Gender { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Avatar { get; set; }
        public Nullable<bool> Status { get; set; }

        public HttpPostedFileBase ImageFile { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EducationFeedback> EducationFeedbacks { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MedicineFeedback> MedicineFeedbacks { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MedicineOrder> MedicineOrders { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ScientificApparatusOrder> ScientificApparatusOrders { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ScientificApparatusFeedback> ScientificApparatusFeedbacks { get; set; }
    }
}
