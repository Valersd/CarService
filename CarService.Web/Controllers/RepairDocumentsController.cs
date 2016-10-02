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
using CarService.Common;
using CarService.Web.ViewModels;
using CarService.Web.Infrastructure.ExtensionMethods;

namespace CarService.Web.Controllers
{
    public class RepairDocumentsController : BaseController
    {
        private int _timeOffset;
        public RepairDocumentsController(ICarServiceData data)
            : base(data)
        {
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (HttpContext.Request.Cookies.AllKeys.Contains("timezoneoffset"))
            {
                var offset = HttpContext.Request.Cookies["timezoneoffset"];
                _timeOffset = -1 * int.Parse(offset.Value);

            }
            base.OnActionExecuting(filterContext);
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
                Data.RepairDocuments.Delete(id.Value);
                Data.SaveChanges();
            }
            catch (Exception)
            {
                return RedirectToAction("Details", new { id = id.Value });
            }
            return Redirect(url);
        }

        public ActionResult ByPart(string part, int? page, string sort)
        {
            var docs = Data
                .RepairDocuments
                .All()
                .Include(d => d.DocumentsParts.Select(dp => dp.Part))
                .Where(d => d.DocumentsParts.Select(dp => dp.Part).Any(p => p.CatalogNumber.Contains(part)));

            var docsModel = GetRepairDocuments(docs, page, sort, DateTime.UtcNow.AddYears(-10), null, String.Empty, String.Empty);

            return View(docsModel);
        }

        public ActionResult Search(int? page, string sort, DateTime? from, DateTime? to, string type, string mechanic)
        {
            var docsModel = GetRepairDocuments(Data.RepairDocuments.All(), page, sort, from, to, type, mechanic);

            ViewBag.Mechanics = GetEmployees(mechanic);
            ViewBag.Types = new List<SelectListItem>()
            {
                new SelectListItem
                {
                    Text = "FINISHED",
                    Value = "finished",
                    Selected = type == "finished"
                },
                new SelectListItem
                {
                    Text = "UNFINISHED",
                    Value = "unfinished",
                    Selected = type == "unfinished"
                }
            };

            return View(docsModel);
        }

        [HttpGet]
        public ActionResult Create()
        {
            var doc = new RepairDocumentCreate();
            PopulateCategoriesAndMechanics(doc, GlobalConstants.InactivRole);
            return View(doc);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RepairDocumentCreate created, string submitButton)
        {
            if (ModelState.IsValid)
            {
                var doc = Mapper.Map<RepairDocument>(created);

                doc.CreatedOn = DateTime.UtcNow.AddMinutes(_timeOffset);
                doc.CreatedById = CurrentUserId;
                if (!string.IsNullOrEmpty(submitButton) && submitButton == "FINISH DOCUMENT")
                {
                    if (doc.TotalPrice.HasValue)
                    {
                        doc.FinishedOn = DateTime.UtcNow.AddMinutes(_timeOffset);
                        foreach (var d in doc.DocumentsParts)
                        {
                            d.UnitPrice = Data.ReplacementParts.GetById(d.PartId).Price;
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("TotalPrice", "Total price should be set when finish document");
                    }
                }
                if (ModelState.IsValid)
                {
                    Data.RepairDocuments.Add(doc);
                    Data.SaveChanges();
                    return RedirectToAction("Details", new { id = doc.Id });
                }
            }
            PopulateCategoriesAndMechanics(created, GlobalConstants.InactivRole);
            created.UsedParts = PopulateCreatedDocumentParts(created.UsedParts);
            created.CarId = null;
            created.CarRegNumber = string.Empty;
            ModelState.Remove("CarId");
            ModelState.Remove("CarRegNumber");
            return View(created);
        }


        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var doc = Data
                .RepairDocuments
                .All()
                .Where(d => d.Id == id.Value)
                .Project()
                .To<RepairDocumentEdit>()
                .FirstOrDefault();

            if (doc == null)
            {
                return HttpNotFound();
            }
            if (doc.CreatedById != CurrentUserId)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }
            PopulateCategoriesAndMechanics(doc, GlobalConstants.InactivRole);
            return View(doc);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(RepairDocumentEdit doc, string submitButton)
        {
            var docPartDbList = Data.DocumentsParts.All().Include(p => p.Part).Where(dp => dp.DocumentId == doc.Id);
            if (ModelState.IsValid)
            {
                var docEdit = Mapper.Map<RepairDocument>(doc);

                if (!string.IsNullOrEmpty(submitButton) && submitButton == "FINISH DOCUMENT")
                {
                    if (docEdit.TotalPrice.HasValue)
                    {
                        docEdit.FinishedOn = DateTime.UtcNow.AddMinutes(_timeOffset);
                        foreach (var d in docEdit.DocumentsParts)
                        {
                            d.UnitPrice = docPartDbList.FirstOrDefault(p => p.PartId == d.PartId).Part.Price;
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("TotalPrice", "Total price should be set when finish document");
                    }
                }
                foreach (var dp in docPartDbList)
                {
                    Data.DocumentsParts.Delete(dp);
                }
                foreach (var d in docEdit.DocumentsParts)
                {
                    d.DocumentId = docEdit.Id;
                    Data.DocumentsParts.Add(d);
                }
                Data.RepairDocuments.Attach(docEdit);
                Data.RepairDocuments.Update(docEdit, d => d.CreatedById, d => d.CreatedOn, d => d.CarId);
                Data.SaveChanges();
                if (ModelState.IsValid)
                {
                    return RedirectToAction("Details", new { id = docEdit.Id });
                }
            }

            PopulateCategoriesAndMechanics(doc, GlobalConstants.InactivRole);
            var parts = docPartDbList.Project().To<PartInRepairDocumentDetails>().OrderBy(d => d.Name).ToList();
            doc.UsedParts = parts;

            return View(doc);
        }

        public ActionResult Details(int? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var doc = Data
                .RepairDocuments
                .All()
                .Where(d => d.Id == id.Value)
                .Project()
                .To<RepairDocumentDetails>()
                .FirstOrDefault();

            //var docFromDb = Data
            //    .RepairDocuments
            //    .All()
            //    .Include(d=>d.Car)
            //    .Include(d=>d.CreatedBy)
            //    .Include(d=>d.Mechanic)
            //    .Include(d => d.DocumentsParts.Select(dp => dp.Part))
            //    .FirstOrDefault(d => d.Id == id.Value);

            //var doc = Mapper.Map<RepairDocumentDetails>(docFromDb);

            if (doc == null)
            {
                return HttpNotFound();
            }

            return View(doc);
        }

        private IPagedList<RepairDocumentSearch> GetRepairDocuments(IQueryable<RepairDocument> docs, int? page, string sort, DateTime? from, DateTime? to, string type, string mechanic)
        {
            ViewBag.Number = string.IsNullOrEmpty(sort) ? "number" : "";
            ViewBag.Created = sort == "created" ? "created_desc" : "created";
            ViewBag.Finished = sort == "finished" ? "finished_desc" : "finished";
            ViewBag.MechanicSort = sort == "mechanic" ? "mechanic_desc" : "mechanic";
            ViewBag.PartsPrice = sort == "parts_price" ? "parts_price_desc" : "parts_price";
            ViewBag.TotalPrice = sort == "total_price" ? "total_price_desc" : "total_price";

            var localDateTime = DateTime.UtcNow.AddMinutes(_timeOffset);
            to = to == null ? new DateTime(localDateTime.Year, localDateTime.Month, localDateTime.Day, 23, 59, 59) : new DateTime(to.Value.AddMinutes(_timeOffset).Year, to.Value.AddMinutes(_timeOffset).Month, to.Value.AddMinutes(_timeOffset).Day, 23, 59, 59);
            from = from == null ? to.Value.AddMonths(-1).AddDays(-1).AddSeconds(2) : new DateTime(from.Value.AddMinutes(_timeOffset).Year, from.Value.AddMinutes(_timeOffset).Month, from.Value.AddMinutes(_timeOffset).Day, 0, 0, 1);
            //docs = Data
            //    .RepairDocuments
            //    .All();

            if (!string.IsNullOrEmpty(mechanic))
            {
                docs = docs.Where(d => d.MechanicId == mechanic);
            }

            switch (type)
            {
                case "finished":
                    docs = docs.Where(d => d.FinishedOn.HasValue && d.FinishedOn.Value >= from && d.FinishedOn.Value <= to);
                    break;
                case "unfinished":
                    docs = docs.Where(d => !d.FinishedOn.HasValue && d.CreatedOn >= from && d.CreatedOn <= to);
                    break;
                default:
                    docs = docs.Where(d => (d.FinishedOn.HasValue && d.FinishedOn >= from && d.FinishedOn.Value <= to) ||
                        (!d.FinishedOn.HasValue && d.CreatedOn >= from && d.CreatedOn <= to));
                    break;
            }

            var docsModel = docs
                .Project()
                .To<RepairDocumentSearch>();

            switch (sort)
            {
                case "number": docsModel = docsModel.OrderBy(d => d.Id); break;
                case "created": docsModel = docsModel.OrderBy(d => d.CreatedOn); break;
                case "created_desc": docsModel = docsModel.OrderByDescending(d => d.CreatedOn); break;
                case "finished": docsModel = docsModel.OrderBy(d => d.FinishedOn); break;
                case "finished_desc": docsModel = docsModel.OrderByDescending(d => d.FinishedOn); break;
                case "mechanic": docsModel = docsModel
                    .OrderBy(d => d.Mechanic.FirstName)
                    .ThenBy(d => d.Mechanic.LastName)
                    .ThenBy(d => d.Id); break;
                case "mechanic_desc": docsModel = docsModel
                    .OrderByDescending(d => d.Mechanic.FirstName)
                    .ThenByDescending(d => d.Mechanic.LastName)
                    .ThenByDescending(d => d.Id); break;
                case "parts_price": docsModel = docsModel.OrderBy(d => d.PartsPrice).ThenBy(d => d.Id); break;
                case "parts_price_desc": docsModel = docsModel.OrderByDescending(d => d.PartsPrice).ThenBy(d => d.Id); break;
                case "total_price": docsModel = docsModel.OrderBy(d => d.TotalPrice).ThenBy(d => d.Id); break;
                case "total_price_desc": docsModel = docsModel.OrderByDescending(d => d.TotalPrice).ThenBy(d => d.Id); break;
                default: docsModel = docsModel.OrderByDescending(d => d.Id); break;
            }

            var totalPrice = HttpContext.Session["TotalPrice"];
            var count = HttpContext.Session["Count"];
            var partsPrice = HttpContext.Session["PartsPrice"];

            if (!string.IsNullOrEmpty(Request["Search"]) || page == null || HttpContext.Session["TotalPrice"] == null)
            {
                totalPrice = docsModel.Sum(d => d.TotalPrice) ?? 0m;
                count = docsModel.Count();
                partsPrice = docsModel.Sum(d => (decimal?)d.PartsPrice) ?? 0m;
            }

            ViewBag.Type = type;
            ViewBag.Mechanic = mechanic;
            ViewBag.From = from.Value.ToLongDateString();
            ViewBag.To = to.Value.ToLongDateString();
            HttpContext.Session["TotalPrice"] = totalPrice;
            HttpContext.Session["Count"] = count;
            HttpContext.Session["PartsPrice"] = partsPrice;

            return docsModel.ToPagedList(page ?? 1, 10);
        }

        private void PopulateCategoriesAndMechanics(RepairDocumentBase doc, params string[] forbiddenRoles)
        {
            if (doc == null)
            {
                return;
            }
            var categories = Data
                .Categories
                .All()
                .OrderBy(c => c.Name)
                .Project()
                .To<CategoryIndex>()
                .ToList();
            doc.Categories = categories;
            doc.Mechanics = GetEmployees(doc.MechanicId, forbiddenRoles);
        }

        private IEnumerable<SelectListItem> GetEmployees(string selectedId, params string[] forbiddenRoles)
        {
            if (selectedId == null)
            {
                selectedId = String.Empty;
            }
            var users = Data
                .Users
                .All();
            if (forbiddenRoles.Length > 0)
            {
                var excludedUserIds = Data
                    .Roles
                    .All()
                    .Where(r => forbiddenRoles.Contains(r.Name))
                    .SelectMany(r => r.Users.Select(u => u.UserId));
                users = users.Where(u => !excludedUserIds.Contains(u.Id));
            }
            var result = users
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                .Project()
                .To<Employee>()
                .ToList()
                .Select(e => new SelectListItem
                {
                    Text = e.FirstName + " " + e.LastName,
                    Value = e.Id,
                    Selected = e.Id == selectedId
                });
            return result;
        }

        private IList<PartInRepairDocumentDetails> PopulateCreatedDocumentParts(IEnumerable<PartInRepairDocumentDetails> posted)
        {
            foreach (var part in posted)
            {
                var dbPart = Data.ReplacementParts.All().Include(p => p.Category).FirstOrDefault(p => p.Id == part.Id);
                part.CatalogNumber = dbPart.CatalogNumber;
                part.Category = dbPart.Category.Name;
                part.Name = dbPart.Name;
                part.IsActive = dbPart.IsActive;
                part.Price = dbPart.Price;
            }
            return posted.OrderBy(p => p.Name).ToList();
        }
    }
}