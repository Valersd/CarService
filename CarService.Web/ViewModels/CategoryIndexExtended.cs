using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

using PagedList;

namespace CarService.Web.ViewModels
{
    public class CategoryIndexExtended
    {
        public int Id { get; set; }

        [Display(Name="Name")]
        public string Name { get; set; }

        [Display(Name = "Count of parts")]
        public int PartsCount { get; set; }

        [Display(Name="Total used parts")]
        public int TotalUsedPartsNumber { get; set; }

        [Display(Name="Amount of used parts ")]
        [DataType(DataType.Currency)]
        public decimal? TotalUsedPartsAmount { get; set; }

        public IPagedList<PartInCategoryDetails> Parts { get; set; }
    }
}