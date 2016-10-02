using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;

namespace CarService.Web.ViewModels
{
    public class PartInRepairDocumentDetails
    {
        public int Id { get; set; }

        [Display(Name="Catalog number")]
        public string CatalogNumber { get; set; }

        [Display(Name="Name")]
        public string Name { get; set; }

        [Display(Name="Category")]
        public string Category { get; set; }

        [Display(Name="Price")]
        [UIHint("CurrencyHidden")]
        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public bool IsActive { get; set; }
    }
}