using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

using AutoMapper;
using AutoMapper.QueryableExtensions;

using PagedList;

using CarService.Data;
using CarService.Models;
using CarService.Web.Models;
using CarService.Web.ViewModels;
using CarService.Web.Infrastructure.ExtensionMethods;

namespace CarService.Web.Controllers
{
    public class CarsController : BaseController
    {
        public CarsController(ICarServiceData data)
            :base(data)
        {
        }

        public ActionResult Search(int? page, string search, string sort)
        {
            ViewBag.CurrentSort = sort;

            ViewBag.LastRepair = string.IsNullOrEmpty(sort) ? "date" : "";
            ViewBag.Number = sort == "number" ? "number_desc" : "number";
            ViewBag.PartsPrice = sort == "parts_price" ? "parts_price_desc" : "parts_price";
            ViewBag.TotalPrice = sort == "total_price" ? "total_price_desc" : "total_price";
            ViewBag.Count = sort == "count" ? "count_desc" : "count";

            var cars = Data
                .Cars
                .All();

            if (!string.IsNullOrEmpty(search))
            {
                ViewBag.Search = search;
                string searchTerm = search.CyrillicToLatinToUpper();
                cars = cars.Where(c=>c.RegNumber.Contains(searchTerm));
            }
            
            var carsModel = cars
                .Project()
                .To<CarSearch>();
            switch (sort)
            {
                case "date": carsModel = carsModel.OrderBy(c => c.LastRepairDateTime); break;
                case "number": carsModel = carsModel.OrderBy(c => c.RegNumber); break;
                case "number_desc": carsModel = carsModel.OrderByDescending(c => c.RegNumber); break;
                case "parts_price": carsModel = carsModel.OrderBy(c => c.PartsPrice); break;
                case "parts_price_desc": carsModel = carsModel.OrderByDescending(c => c.PartsPrice); break;
                case "total_price": carsModel = carsModel.OrderBy(c => c.TotalPrice); break;
                case "total_price_desc": carsModel = carsModel.OrderByDescending(c => c.TotalPrice); break;
                case "count": carsModel = carsModel.OrderBy(c => c.RepairsCount).ThenBy(c => c.RegNumber); break;
                case "count_desc": carsModel = carsModel.OrderByDescending(c => c.RepairsCount).ThenBy(c => c.RegNumber); break;
                default: carsModel = carsModel.OrderByDescending(c => c.LastRepairDateTime); break;
            }
            var carsPaged = carsModel.ToPagedList(page ?? 1, 10);
            ViewBag.ShowPager = carsPaged.TotalItemCount > carsPaged.Count;

            return View(carsPaged);
        }

        public ActionResult Details(int? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var car = Data
                .Cars
                .All()
                .Where(c => c.Id == id.Value)
                .Project()
                .To<CarDetails>()
                .FirstOrDefault();

            if (car == null)
            {
                return HttpNotFound();
            }
            return View(car);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int? id)
        {
            var url = TempData["Url"] as string;
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            try
            {
                Data.Cars.Delete(id.Value);
                Data.SaveChanges();
            }
            catch (Exception)
            {
                return RedirectToAction("Details", new { id = id.Value });
            }
            return Redirect(url);
        }
    }
}