using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarService.Web.ViewModels
{
    public class PartSearch : PartInCategoryDetails
    {
        public int Id { get; set; }

        [Display(Name = "Catalog number")]
        public string CatalogNumber { get; set; }

        [Display(Name = "Category")]
        [UIHint("_CutLargeText")]
        [AdditionalMetadata("Length", 33)]
        public string Category { get; set; }

    }
}