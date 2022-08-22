using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StudentAffiairs.Models
{
    [MetadataType(typeof(PasscodeValidationMetaData))]
    public partial  class PasscodeValidation
    {

    }
    public  class PasscodeValidationMetaData
    {
        [Display(Name = "Access Number")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Access Number field required")]
        public string AccessNumber { get; set; } 
        [Display(Name = "Security Passcode")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Passcode field required")]
        [MinLength(5, ErrorMessage = "invalid  Security Passcode length")]
        [MaxLength(6, ErrorMessage = "Maximum length reached")]
        public string Passcode { get; set; }
    }
}