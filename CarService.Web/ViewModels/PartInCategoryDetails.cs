using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarService.Web.ViewModels
{
    public class PartInCategoryDetails
    {
        [Display(Name = "Name")]
        [UIHint("_CutLargeText")]
        [AdditionalMetadata("Length", 35)]
        public string Name { get; set; }

        [Display(Name = "Current price")]
        [DataType(DataType.Currency)]
        public decimal CurrentPrice { get; set; }

        [Display(Name = "Total used number")]
        public int TotalUsedNumber { get; set; }

        [Display(Name = "Total amount")]
        [DataType(DataType.Currency)]
        public decimal TotalAmount { get; set; }

        public bool IsActive { get; set; }
    }
}