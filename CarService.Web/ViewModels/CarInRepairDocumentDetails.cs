using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CarService.Web.ViewModels
{
    public class CarInRepairDocumentDetails
    {
        public int Id { get; set; }

        [Display(Name="Registration number")]
        public string RegNumber { get; set; }

        [Display(Name="Chassis number")]
        public string ChassisNumber { get; set; }

        [Display(Name = "Engine number")]
        public string EngineNumber { get; set; }

        [Display(Name="Vendor")]
        [DisplayFormat(NullDisplayText="unknown")]
        public string Vendor { get; set; }

        [Display(Name="Model")]
        [DisplayFormat(NullDisplayText="unknown")]
        public string Model { get; set; }

        [Display(Name = "Owner")]
        [DisplayFormat(NullDisplayText = "unknown")]
        public string OwnerName { get; set; }

        [Display(Name = "Phone")]
        [DataType(DataType.PhoneNumber)]
        public string OwnerPhone { get; set; }

        [Display(Name = "Year of manufacture")]
        [DisplayFormat(NullDisplayText = "unrecorded")]
        public int? Year { get; set; }

        [Display(Name = "Engine capacity")]
        [DisplayFormat(DataFormatString = "{0} cm3", NullDisplayText = "unrecorded")]
        public int? EngineCapacity { get; set; }

        [Display(Name = "Color")]
        [DisplayFormat(NullDisplayText = "unrecorded")]
        public string Color { get; set; }

        [Display(Name = "Car description")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
    }
}