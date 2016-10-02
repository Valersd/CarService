using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using AutoMapper;
using AutoMapper.QueryableExtensions;

using PagedList;

using CarService.Data;
using CarService.Models;
using CarService.Web.ViewModels;

namespace CarService.Web.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(ICarServiceData data)
            :base(data)
        {
        }
        public ActionResult Index(int? page, string sort)
        {
            ViewBag.CurrentSort = sort;

            ViewBag.Number = string.IsNullOrEmpty(sort) ? "id_desc" : "";
            ViewBag.Car = sort == "car" ? "car_desc" : "car";
            ViewBag.Description = sort == "description" ? "description_desc" : "description";
            ViewBag.CreatedBy = sort == "created_by" ? "created_by_desc" : "created_by";
            ViewBag.Mechanic = sort == "mechanic" ? "mechanic_desc" : "mechanic";

            var docs = Data
                .RepairDocuments
                .All()
                .Where(d => d.FinishedOn == null);

            switch (sort)
            {
                case "id_desc": docs = docs.OrderByDescending(d => d.Id); break;
                case "car": docs = docs.OrderBy(d => d.Car.RegNumber); break;
                case "car_desc": docs = docs.OrderByDescending(d => d.Car.RegNumber); break;
                case "description": docs = docs.OrderBy(d => d.RepairDescription); break;
                case "description_desc": docs = docs.OrderByDescending(d => d.RepairDescription); break;
                case "created_by": docs = docs.OrderBy(d => d.CreatedBy.FirstName).ThenBy(d => d.CreatedBy.LastName); break;
                case "created_by_desc": 
                    docs = docs.OrderByDescending(d => d.CreatedBy.FirstName).ThenByDescending(d => d.CreatedBy.LastName); 
                    break;
                case "mechanic": docs = docs.OrderBy(d => d.Mechanic.FirstName).ThenBy(d => d.Mechanic.LastName); break;
                case "mechanic_desc": 
                    docs = docs.OrderByDescending(d => d.Mechanic.FirstName).ThenByDescending(d => d.Mechanic.LastName); 
                    break;
                default: docs = docs.OrderBy(d => d.Id); break;
            }

            var result = docs
                .Project()
                .To<RepairDocumentHomeIndex>()
                .ToPagedList(page ?? 1, 10);

            return View(result);
        }

    }
}