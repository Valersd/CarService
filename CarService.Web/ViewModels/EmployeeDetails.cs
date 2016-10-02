using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

using PagedList;

namespace CarService.Web.ViewModels
{
    public class EmployeeDetails : EmployeeIndex
    {
        [Display(Name="Phone number")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Display(Name="Created documents")]
        public IEnumerable<RepairDocumentSearch> CreatedRepairDocuments { get; set; }

        [Display(Name="Served documents")]
        public IEnumerable<RepairDocumentSearch> ServedRepairDocuments { get; set; }
    }
}