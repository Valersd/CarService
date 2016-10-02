using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

using CarService.Web.Infrastructure.Validation;

namespace CarService.Web.ViewModels
{
    public class CarBase
    {
        [StringLength(20, ErrorMessage = "{0} should be no more than {1} symbols")]
        public virtual string Vendor { get; set; }

        [StringLength(20, ErrorMessage = "{0} should be no more than {1} symbols")]
        public virtual string Model { get; set; }

        [Display(Name = "Year of manufacture")]
        [YearRange(1975, ErrorMessage = "{0} should be between {1} and {2}")]
        public virtual int? Year { get; set; }

        [StringLength(20, MinimumLength = 2, ErrorMessage = "{0} should be between {1} and {2} symbols")]
        public virtual string Color { get; set; }

        [Display(Name = "Engine capacity (cm3)")]
        [Range(50, 10000, ErrorMessage = "Engine capacity should be between {1} and {2} cm3")]
        public virtual int? EngineCapacity { get; set; }

        [StringLength(3000, ErrorMessage = "{0} should be no more than {1} symbols")]
        [DataType(DataType.MultilineText)]
        public virtual string Description { get; set; }

        [Display(Name = "Owner")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "{0} should be between {2} and {1} symbols")]
        public virtual string OwnerName { get; set; }

        [Display(Name = "Phone")]
        [StringLength(15, MinimumLength = 4, ErrorMessage = "{0} should be between {2} and {1} symbols")]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Only numbers allowed")]
        public virtual string OwnerPhone { get; set; }
    }
}