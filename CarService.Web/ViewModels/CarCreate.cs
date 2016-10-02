using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarService.Web.ViewModels
{
    public class CarCreate : CarBase
    {
        [Required]
        [Display(Name="Registration number")]
        [StringLength(10, MinimumLength=4, ErrorMessage="{0} should be between {2} and {1} symbols")]
        [Remote("IsRegNumberUnique", "Validation", ErrorMessage="Registration number already exists")]
        public string RegNumber { get; set; }

        [Required]
        [Display(Name="Chassis number")]
        [Remote("IsChassisUnique", "Validation", ErrorMessage = "Chassis number already exists")]
        [StringLength(20, MinimumLength = 10, ErrorMessage = "{0} should be between {2} and {1} symbols")]
        public string ChassisNumber { get; set; }

        [Required]
        [Display(Name="Engine number")]
        [Remote("IsEngineUnique", "Validation", ErrorMessage = "Engine number already exists")]
        [StringLength(20, MinimumLength = 10, ErrorMessage = "{0} should be between {2} and {1} symbols")]
        public string EngineNumber { get; set; }
    }
}