using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CarService.Web.ViewModels
{
    public class RepairDocumentSearch
    {
        [Display(Name = "№")]
        [DisplayFormat(DataFormatString = "{0:000000}")]
        public int Id { get; set; }

        [Display(Name = "Created on")]
        [DataType(DataType.DateTime)]
        public DateTime CreatedOn { get; set; }

        [Display(Name = "Finished on")]
        [DataType(DataType.DateTime)]
        public DateTime? FinishedOn { get; set; }

        [Display(Name = "Mechanic")]
        public Employee Mechanic { get; set; }

        [Display(Name = "Total price")]
        [DataType(DataType.Currency)]
        public decimal? TotalPrice { get; set; }

        [Display(Name = "Price of parts")]
        [DataType(DataType.Currency)]
        public decimal PartsPrice { get; set; }
    }
}