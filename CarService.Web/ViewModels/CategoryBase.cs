using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace CarService.Web.ViewModels
{
    public class CategoryBase
    {
        [Required]
        [Display(Name = "Name")]
        [StringLength(50, MinimumLength=2, ErrorMessage="{0} should be between {2} and {1} symbols")]
        [Remote("IsCategoryNameUniqueCreate", "Validation", ErrorMessage="Name already exists")]
        public virtual string Name { get; set; }
    }
}
