using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CarService.Web.ViewModels
{
    public class CarDetails : CarInRepairDocumentDetails
    {
        [Display(Name = "Repair documents")]
        public IEnumerable<RepairDocumentDetails> RepairDocuments { get; set; }
    }
}