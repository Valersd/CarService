using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CarService.Web.ViewModels
{
    public class EmployeeEdit
    {
        public string Id { get; set; }

        [Required]
        [Display(Name = "First name")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "{0} should be between {2} and {1} symbols")]
        [RegularExpression(@"^[A-zА-я]+$", ErrorMessage = "Only letters allowed")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last name")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "{0} should be between {2} and {1} symbols")]
        [RegularExpression(@"^[A-zА-я]+$", ErrorMessage = "Only letters allowed")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Phone number")]
        [StringLength(15, MinimumLength = 4, ErrorMessage = "{0} should be between {2} and {1} symbols")]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Only numbers allowed")]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(40, MinimumLength = 3, ErrorMessage = "{0} should be between {2} and {1} symbols")]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Role")]
        public string Role { get; set; }

        [Display(Name="Current role")]
        public string CurrentRole { get; set; }
    }
}