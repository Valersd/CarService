using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

using AutoMapper;
using AutoMapper.QueryableExtensions;

using CarService.Data;
using CarService.Models;
using CarService.Common;
using CarService.Web.ViewModels;
using System.Data.Entity.Infrastructure;

namespace CarService.Web.Controllers
{
    public class CategoriesController : BaseController
    {
        public CategoriesController(ICarServiceData data)
            : base(data)
        {
        }

        [HttpGet]
        [Authorize(Roles = GlobalConstants.AdminRole)]
        public ActionResult Edit(int? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var cat = Data.Categories.GetById(id.Value);
            if (cat == null)
            {
                return HttpNotFound();
            }

            var model = Mapper.Map<CategoryIndex>(cat);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = GlobalConstants.AdminRole)]
        public ActionResult Edit(CategoryIndex cat)
        {
            if (ModelState.IsValid)
            {
                var edited = Mapper.Map<Category>(cat);
                try
                {
                    Data.Categories.Update(edited);
                    Data.SaveChanges();
                    TempData["Message"] = edited.Name + " successfully updated";
                    TempData["CssClass"] = "text-success";
                    return RedirectToAction("Index");
                }
                catch(DbUpdateException ex)
                {
                    HandleDbUpdateException(ex);
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Some error occurred while saving");
                }
            }
            return View(cat);
        }

        [HttpGet]
        [Authorize(Roles = GlobalConstants.AdminRole)]
        public ActionResult Create()
        {
            return View(new CategoryBase());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = GlobalConstants.AdminRole)]
        public ActionResult Create(CategoryBase category)
        {
            if (ModelState.IsValid)
            {
                var categoryNew = Mapper.Map<Category>(category);
                try
                {
                    Data.Categories.Add(categoryNew);
                    Data.SaveChanges();
                    TempData["Message"] = categoryNew.Name + " successfully added";
                    TempData["CssClass"] = "text-success";
                    return RedirectToAction("Index");
                }
                catch (DbUpdateException ex)
                {
                    HandleDbUpdateException(ex);
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Some error occurred while saving");
                }
            }
            return View(category);
        }

        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult Index()
        {
            var categories = Data
                .Categories
                .All()
                .OrderBy(c => c.Name)
                .Project()
                .To<CategoryIndex>()
                .AsEnumerable();

            return View(categories);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = GlobalConstants.AdminRole)]
        public ActionResult Delete(int? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (Data.ReplacementParts.All().Include(p => p.DocumentsParts).Any(p => p.CategoryId == id.Value && p.DocumentsParts.Count > 0))
            {
                TempData["Message"] = "First you have to delete all documents that contain replacement parts belonging to this category";
                TempData["CssClass"] = "text-danger";
                return RedirectToAction("Index");
            }
            try
            {
                Data.Categories.Delete(id);
                Data.SaveChanges();
                TempData["Message"] = "Category successfully deleted";
                TempData["CssClass"] = "text-success";
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return RedirectToAction("Index");
        }
    }
}