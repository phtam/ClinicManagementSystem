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
    using System.ComponentModel.DataAnnotations;
    using System.Web;

    public partial class Education
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Education()
        {
            this.EducationFeedbacks = new HashSet<EducationFeedback>();
        }

        public int EducationID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter lesson name !")]
        [StringLength(maximumLength: 150, MinimumLength = 5, ErrorMessage = "Lesson name must be between 5 to 150 charaters !")]
        [DisplayName("Lesson Name")]
        public string LessonName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please choice activity !")]
        [DisplayName("Activity ID")]
        public Nullable<int> ActivityID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please choice subject !")]
        [DisplayName("Subject ID")]
        public Nullable<int> SubjectID { get; set; }

        [DataType(DataType.DateTime, ErrorMessage = "Created date is incorrect format !")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm:ss}")]
        public Nullable<System.DateTime> CreateDate { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter content !")]
        [DisplayName("Content")]
        public string Content { get; set; }

        public string Thumbnail { get; set; }

        public HttpPostedFileBase ImageFile { get; set; }

        public virtual Activity Activity { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EducationFeedback> EducationFeedbacks { get; set; }
        public virtual Subject Subject { get; set; }
    }
}
