using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;


namespace CarService.Web.ViewModels
{
    public class CategoryIndex : CategoryBase
    {
        public int Id { get; set; }

        [Remote("IsCategoryNameUniqueEdit", "Validation", ErrorMessage="Name already exists", AdditionalFields="Id")]
        public override string Name { get; set; }
    }
}