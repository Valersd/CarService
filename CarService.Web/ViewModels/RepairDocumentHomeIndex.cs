using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

using CarService.Models;

namespace CarService.Web.ViewModels
{
    public class RepairDocumentHomeIndex
    {
        [Display(Name="Number")]
        [DisplayFormat(DataFormatString="{0:000000}")]
        public int Id { get; set; }

        //[Display(Name="Created on")]
        //[DisplayFormat(DataFormatString="{0:g}")]
        //public DateTime CreatedOn { get; set; }

        [Display(Name="Car")]
        public string CarRegNumber { get; set; }

        //[Display(Name="Replacement parts")]
        //public IEnumerable<ReplacementPart> Parts { get; set; }

        //[Display(Name="Price of parts")]
        //[DisplayFormat(DataFormatString="{0:C}")]
        //public decimal PartsPrice { get; set; }

        [Display(Name="Repair description")]
        [UIHint("_CutLargeText")]
        [AdditionalMetadata("Length", 80)]
        public string RepairDescription { get; set; }

        [Display(Name="Created by")]
        public Employee CreatedBy { get; set; }

        [Display(Name="Mechanic")]
        public Employee Mechanic { get; set; }
    }
}