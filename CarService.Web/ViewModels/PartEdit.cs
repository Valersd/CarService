using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarService.Web.ViewModels
{
    public class PartEdit : PartBase
    {
        public int Id { get; set; }

        [Remote("IsCatalogNumberUniqueEdit", "Validation", ErrorMessage = "Catalog number already exists", AdditionalFields="Id")]
        public override string CatalogNumber { get; set; }
    }
}