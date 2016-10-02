using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Net;
using System.Web;
using System.Web.Mvc;

using System.Data.Entity;
using System.Data.Entity.Infrastructure;

using AutoMapper;
using AutoMapper.QueryableExtensions;

using PagedList;

using CarService.Data;
using CarService.Models;
using CarService.Web.ViewModels;
using CarService.Web.Infrastructure.ExtensionMethods;

namespace CarService.Web.Controllers
{
    [OutputCache(NoStore=true, Duration=0, VaryByParam="*")]
    public class AjaxController : BaseController
    {
        public AjaxController(ICarServiceData data)
            : base(data)
        {
        }

        public ActionResult CategoryDetails(int id, int? page)
        {
            if (Request.IsAjaxRequest())
            {
                var category = Data
                .Categories
                .All()
                .Where(c => c.Id == id)
                .Project()
                .To<CategoryIndexExtended>()
                .FirstOrDefault();

                var parts = Data
                    .ReplacementParts
                    .All()
                    .Where(p => p.CategoryId == id)
                    .Project()
                    .To<PartInCategoryDetails>()
                    .OrderBy(p => p.Name)
                    .ToPagedList(page ?? 1, 10);

                category.Parts = parts;

                return PartialView("_CategoryDetails", category);
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        public ActionResult FindCatalogNumber(string term)
        {
            if (Request.IsAjaxRequest())
            {
                var catalogNumbers = Data
                    .ReplacementParts
                    .All()
                    .Where(p => p.CatalogNumber.Contains(term))
                    .Select(p => p.CatalogNumber);
                return Json(catalogNumbers, JsonRequestBehavior.AllowGet);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [HttpGet]
        public ActionResult GetCategoryParts(int catId)
        {
            if (Request.IsAjaxRequest())
            {
                var parts = Data
                    .ReplacementParts
                    .All()
                    .Where(p => p.CategoryId == catId)
                    .OrderBy(p => p.Name)
                    .Project()
                    .To<PartInRepairDocumentDetails>()
                    .AsEnumerable();

                if (parts != null)
                {
                    return PartialView("_PartsInEditRepairDocumentAdd", parts);
                }
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        public ActionResult GetCar(int? id)
        {
            if (Request.IsAjaxRequest() && id.HasValue)
            {
                var car = Data.Cars.GetById(id.Value);
                if (car == null)
                {
                    return HttpNotFound();
                }
                var carModel = Mapper.Map<CarInRepairDocumentDetails>(car);

                return PartialView("/Views/Shared/DisplayTemplates/CarInRepairDocumentDetails.cshtml", carModel);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [HttpGet]
        public ActionResult EditCar(int? id)
        {
            if (Request.IsAjaxRequest() && id.HasValue)
            {
                var car = Data.Cars.GetById(id);
                if (car == null)
                {
                    return HttpNotFound();
                }
                var carModel = Mapper.Map<CarEdit>(car);
                return PartialView("/Views/RepairDocuments/CarEdit.cshtml", carModel);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCar(CarEdit car)
        {
            if (ModelState.IsValid)
            {
                var carEdited = Mapper.Map<Car>(car);
                Data.Cars.Attach(carEdited);
                Data.Cars.Update(carEdited, c => c.RegNumber, c => c.ChassisNumber, c => c.EngineNumber);
                try
                {
                    Data.SaveChanges();
                    return Json(new { cssClass = "text-success", message = "Changes saved successfully" });
                }
                catch(DbUpdateException ex)
                {
                    HandleDbUpdateException(ex);
                }
                catch (Exception)
                {
                    return Json(new { cssClass = "text-danger", message = "Some error occurred while saving" });
                }
            }
            return Json(new { cssClass = "text-danger", message = String.Join(Environment.NewLine, ModelState.Values.SelectMany(v => v.Errors).Select(x => x.ErrorMessage)) });
        }

        public ActionResult FindCar(string term)
        {
            if (Request.IsAjaxRequest())
            {
                string searchTerm = term.CyrillicToLatinToUpper();
                var cars = Data
                    .Cars
                    .All()
                    .Where(c => c.RegNumber.Contains(searchTerm))
                    .OrderBy(c => c.RegNumber)
                    .Select(c => new
                    {
                        value = c.Id,
                        label = c.RegNumber
                    })
                    .ToList();
                return Json(cars, JsonRequestBehavior.AllowGet);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [HttpGet]
        public ActionResult CreateCar()
        {
            if (Request.IsAjaxRequest())
            {
                return PartialView("/Views/RepairDocuments/CarCreate.cshtml", new CarCreate());
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCar(CarCreate car)
        {
            if (ModelState.IsValid)
            {
                var carCreated = Mapper.Map<Car>(car);
                Data.Cars.Add(carCreated);
                try
                {
                    Data.SaveChanges();
                    var id = carCreated.Id;
                    var number = carCreated.RegNumber;
                    return Json(new { cssClass = "text-success", message = number + " saved successfully", id = id, number = number });
                }
                catch (DbUpdateException ex)
                {
                    HandleDbUpdateException(ex);
                }
                catch (Exception)
                {
                    return Json(new { cssClass = "text-danger", message = "Some error occurred while saving" });
                }
            }
            return Json(new { cssClass = "text-danger", message = String.Join(Environment.NewLine, ModelState.Values.SelectMany(v => v.Errors).Select(x => x.ErrorMessage)) });
        }

        public ActionResult FindDocumentByNumber(string term)
        {
            if (Request.IsAjaxRequest())
            {
                int id;
                if (int.TryParse(term, out id))
                {
                    var result = Data
                        .RepairDocuments
                        .All()
                        .Where(d => d.Id == id)
                        .Select(d => new
                        {
                            value = d.Id,
                            label = d.Id
                        })
                        .ToList();
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { }, JsonRequestBehavior.AllowGet);
                }
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);            
        }
    }
}