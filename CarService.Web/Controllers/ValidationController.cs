using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;

using CarService.Data;
using CarService.Models;
using CarService.Web.Infrastructure.ExtensionMethods;

namespace CarService.Web.Controllers
{
    [OutputCache(Location = System.Web.UI.OutputCacheLocation.None, NoStore = true)]
    public class ValidationController : BaseController
    {
        public ValidationController(ICarServiceData data)
            : base(data)
        {
        }

        public ActionResult IsCategoryNameUniqueCreate(string name)
        {
            if (Data.Categories.All().Count(c=>c.Name == name) > 0)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public ActionResult IsCategoryNameUniqueEdit(string name, int id)
        {
            if (Data.Categories.All().Count(c => c.Name == name && c.Id != id) > 0)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public ActionResult IsCatalogNumberUniqueEdit(string catalogNumber, int id)
        {
            if (Data.ReplacementParts.All().Count(p => p.CatalogNumber == catalogNumber && p.Id != id) > 0)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public ActionResult IsCatalogNumberUniqueCreate(string catalogNumber)
        {
            if (Data.ReplacementParts.All().Count(p => p.CatalogNumber == catalogNumber) > 0)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public ActionResult IsRegNumberUnique(string regNumber)
        {
            string regNumberLatin = regNumber.CyrillicToLatinToUpper();
            if (Data.Cars.All().Count(c => c.RegNumber == regNumberLatin) > 0)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public ActionResult IsChassisUnique(string chassisNumber)
        {
            string chassiss = chassisNumber.CyrillicToLatinToUpper();
            if (Data.Cars.All().Count(c => c.ChassisNumber == chassiss) > 0)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public ActionResult IsEngineUnique(string engineNumber)
        {
            string engine = engineNumber.CyrillicToLatinToUpper();
            if (Data.Cars.All().Count(c => c.EngineNumber == engine) > 0)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}