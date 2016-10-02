using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.ComponentModel.DataAnnotations;

using CarService.Models;

namespace CarService.Web.ViewModels
{
    public class RepairDocumentEdit : RepairDocumentBase
    {
        public RepairDocumentEdit()
            : base()
        {
        }

        [Display(Name = "№")]
        [DisplayFormat(DataFormatString = "{0:000000}")]
        public int Id { get; set; }

        [Display(Name = "Created on")]
        [UIHint("_DateTimeDisabled")]
        public DateTime CreatedOn { get; set; }

        public int CarId { get; set; }

        [Display(Name="Car")]
        [UIHint("_StringReadonly")]
        public override string CarRegNumber { get; set; }

        public string CreatedById { get; set; }

    }
}