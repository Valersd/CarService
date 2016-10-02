using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CarService.Web.ViewModels
{
    public class EmployeeIndex: Employee
    {
        [Display(Name="Username")]
        public string UserName { get; set; }

        [Display(Name="Full name")]
        public string FullName
        {
            get { return this.FirstName + " " + this.LastName; }
        }

        [Display(Name="Role")]
        public string Role { get; set; }

        [Display(Name="Documents created")]
        public int DocumentsCreatedCount { get; set; }

        [Display(Name="Total amount created")]
        [DataType(DataType.Currency)]
        public decimal DocumentsCreatedTotalAmount { get; set; }

        [Display(Name = "Documents served")]
        public int DocumentsServedCount { get; set; }

        [Display(Name = "Total amount served")]
        [DataType(DataType.Currency)]
        public decimal DocumentsServedTotalAmount { get; set; }
    }
}