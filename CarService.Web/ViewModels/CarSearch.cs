using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CarService.Web.ViewModels
{
    public class CarSearch
    {
        public int Id { get; set; }

        [Display(Name="Car number")]
        public string RegNumber { get; set; }

        [Display(Name="Count of repairs")]
        public int RepairsCount { get; set; }

        [Display(Name="Price of parts")]
        [DataType(DataType.Currency)]
        public decimal PartsPrice { get; set; }

        [Display(Name="Total price")]
        [DataType(DataType.Currency)]
        public decimal? TotalPrice { get; set; }

        [Display(Name="Last repair")]
        public DateTime? LastRepairDateTime { get; set; }
    }
}