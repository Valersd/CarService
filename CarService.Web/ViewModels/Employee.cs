using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;

namespace CarService.Web.ViewModels
{
    public class Employee
    {
        public string Id { get; set; }

        [Display(Name="First name")]
        public string FirstName { get; set; }

        [Display(Name="Last name")]
        public string LastName { get; set; }
    }
}