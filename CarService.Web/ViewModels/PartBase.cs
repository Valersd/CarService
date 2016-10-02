using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarService.Web.ViewModels
{
    public class PartBase
    {
        [Required]
        [Display(Name="Catalog number")]
        [StringLength(30,MinimumLength=5, ErrorMessage="{0} should be between {2} and {1} symbols")]
        [Remote("IsCatalogNumberUniqueCreate", "Validation", ErrorMessage = "Catalog number already exists")]
        public virtual string CatalogNumber { get; set; }

        [Required]
        [Display(Name = "Name")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "{0} should be between {2} and {1} symbols")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Price")]
        [DataType(DataType.Currency)]
        [Range(0.01, 10000000, ErrorMessage = "Price should be positive")]
        public decimal? Price { get; set; }

        [Display(Name="In stock")]
        public bool IsActive { get; set; }

        [Required]
        [Display(Name="Category")]
        public int CategoryId { get; set; }
    }
}