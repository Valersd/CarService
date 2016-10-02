using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

using System.Data.Entity;
using System.Data.Entity.Infrastructure;

using AutoMapper;
using AutoMapper.QueryableExtensions;

using PagedList;

using CarService.Data;
using CarService.Models;
using CarService.Common;
using CarService.Web.ViewModels;

namespace CarService.Web.Controllers
{
    public class PartsController : BaseController
    {
        public PartsController(ICarServiceData data)
            : base(data)
        {
        }

        [HttpGet]
        [Authorize(Roles=GlobalConstants.AdminRole)]
        public ActionResult Edit(int? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var part = Data.ReplacementParts.GetById(id.Value);
            if (part == null)
            {
                return HttpNotFound();
            }

            var model = Mapper.Map<PartEdit>(part);
            ViewBag.Categories = GetCategories(model.CategoryId);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = GlobalConstants.AdminRole)]
        public ActionResult Edit(PartEdit edited)
        {
            if (ModelState.IsValid)
            {
                var url = TempData["Url"] as string;
                var part = Mapper.Map<ReplacementPart>(edited);
                try
                {
                    Data.ReplacementParts.Update(part);
                    Data.SaveChanges();
                    return Redirect(url);
                }
                catch (DbUpdateException ex)
                {
                    HandleDbUpdateException(ex);
                }
            }
            ViewBag.Categories = GetCategories(edited.CategoryId);
            return View(edited);
        }

        [HttpGet]
        [Authorize(Roles = GlobalConstants.AdminRole)]
        public ActionResult Create()
        {
            ViewBag.Categories = GetCategories();
            return View(new PartBase() { IsActive = true });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = GlobalConstants.AdminRole)]
        public ActionResult Create(PartBase created)
        {
            if (ModelState.IsValid)
            {
                var url = TempData["Url"] as string;
                var part = Mapper.Map<ReplacementPart>(created);
                try
                {
                    Data.ReplacementParts.Add(part);
                    Data.SaveChanges();
                    return Redirect(url);
                }
                catch (DbUpdateException ex)
                {
                    HandleDbUpdateException(ex);
                }
            }

            ViewBag.Categories = GetCategories();
            return View(created);
        }

        public ActionResult Search(int? page, string search, string sort, string stock, int catId = 0, int count = 10)
        {
            ViewBag.Name = string.IsNullOrEmpty(sort) ? "name_desc" : "";
            ViewBag.Number = sort == "number" ? "number_desc" : "number";
            ViewBag.Category = sort == "category" ? "category_desc" : "category";
            ViewBag.Price = sort == "price" ? "price_desc" : "price";
            ViewBag.Used = sort == "used" ? "used_desc" : "used";
            ViewBag.TotalPrice = sort == "total_price" ? "total_price_desc" : "total_price";

            var parts = Data
                .ReplacementParts
                .All();

            if (catId > 0)
            {
                parts = parts.Where(p => p.CategoryId == catId);
            }

            if (!string.IsNullOrEmpty(search))
            {
                parts = parts.Where(p => p.Name.Contains(search) || p.CatalogNumber.Contains(search));
            }

            if (!string.IsNullOrEmpty(stock))
            {
                parts = parts.Where(p => stock == "IN" ? p.IsActive : !p.IsActive);
            }

            var partsModel = parts
                .Project()
                .To<PartSearch>();

            switch (sort)
            {
                case "name_desc": partsModel = partsModel.OrderByDescending(p => p.Name); break;
                case "category": partsModel = partsModel.OrderBy(p => p.Category); break;
                case "category_desc": partsModel = partsModel.OrderByDescending(p => p.Category); break;
                case "number": partsModel = partsModel.OrderBy(p => p.CatalogNumber); break;
                case "number_desc": partsModel = partsModel.OrderByDescending(p => p.CatalogNumber); break;
                case "price": partsModel = partsModel.OrderBy(p => p.CurrentPrice); break;
                case "price_desc": partsModel = partsModel.OrderByDescending(p => p.CurrentPrice); break;
                case "used": partsModel = partsModel.OrderBy(p => p.TotalUsedNumber); break;
                case "used_desc": partsModel = partsModel.OrderByDescending(p => p.TotalUsedNumber); break;
                case "total_price": partsModel = partsModel.OrderBy(p => p.TotalAmount); break;
                case "total_price_desc": partsModel = partsModel.OrderByDescending(p => p.TotalAmount); break;
                default: partsModel = partsModel.OrderBy(p => p.Name); break;
            }

            ViewBag.Categories = GetCategories(catId);
            var list = GetResultsPerPage();
            ViewBag.Results = list;
            ViewBag.Stock = new List<SelectListItem>()
            {
                new SelectListItem
                {
                    Text = "IN",
                    Value = "IN",
                    Selected = stock == "IN"
                },
                new SelectListItem
                {
                    Text = "OUT",
                    Value = "OUT",
                    Selected = stock == "OUT"
                }
            };
            return View(partsModel.ToPagedList(page ?? 1, count));
        }

        private IEnumerable<SelectListItem> GetCategories(int selected = 0)
        {
            var categories = Data
                .Categories
                .All()
                .OrderBy(c => c.Name)
                .ToList()
                .Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString(),
                    Selected = c.Id == selected
                });
            return categories;
        }

        private IEnumerable<SelectListItem> GetResultsPerPage()
        {
            return new List<SelectListItem>()
            {
                new SelectListItem
                {
                    Text = "5",
                    Value = "5",
                },
                new SelectListItem
                {
                    Text = "10",
                    Value = "10",
                },
                new SelectListItem
                {
                    Text = "20",
                    Value = "20",
                },
                new SelectListItem
                {
                    Text = "50",
                    Value = "50",
                },
            };
        }
    }
}