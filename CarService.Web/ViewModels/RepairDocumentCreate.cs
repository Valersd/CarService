using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CarService.Web.ViewModels
{
    public class RepairDocumentCreate : RepairDocumentBase
    {
        public RepairDocumentCreate()
            : base()
        {
        }

        [Required(AllowEmptyStrings=false, ErrorMessage="You should choose or add car")]
        public int? CarId { get; set; }

        [Display(Name = "Car number")]
        public override string CarRegNumber { get; set; }
    }
}