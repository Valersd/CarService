using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CarService.Web.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [StringLength(40, MinimumLength=3)]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [Display(Name="First name")]
        [StringLength(20, MinimumLength=2, ErrorMessage="{0} should be between {2} and {1} symbols")]
        [RegularExpression(@"^[A-zА-я]+$", ErrorMessage = "Only letters allowed")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name="Last name")]
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
        [StringLength(100, MinimumLength = 6, ErrorMessage = "{0} should be between {2} and {1} symbols")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name="Role")]
        public string Role { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
