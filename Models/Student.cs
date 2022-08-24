//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace sheduler.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Student
{
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Student()
        {
            this.Userroles = new HashSet<Userrole>();
        }
        [Display(Name = " ACCESS NUMBER")]
        public string AccessNumber { get; set; }
        [Display(Name = "USERNAME")]
        [Required(AllowEmptyStrings = false, ErrorMessage = " USERNAME field required")]
        [MinLength(10, ErrorMessage = "invalid UserName length")]

        public string UserName { get; set; }
        [Display(Name = "EMAIL")]
        [Required(AllowEmptyStrings = false, ErrorMessage = " EMAIL field required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Display(Name = "COURSE")]
        public int Course_code { get; set; }
        [Display(Name = "SEMESTER ")]
        public int sem_id { get; set; }
        [Display(Name = "COUNTRY")]
        public int Year_Id { get; set; }
        public int campus_id { get; set; }
        [Display(Name = "YEAR")]
        [Required(AllowEmptyStrings = false, ErrorMessage = " COUNTRY field required")]
        [MinLength(10, ErrorMessage = "invalid COUNTRY length")]
     
        public string Country { get; set; }
        public string Passcode { get; set; }
        public virtual Campus_Branches Campus_Branches { get; set; }
        public virtual Cours Cours { get; set; }
        public virtual semester semester { get; set; }
        public virtual UserLocation UserLocation { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Userrole> Userroles { get; set; }
        public virtual Year Year { get; set; }
    }
}