using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;

using System.Data.Entity;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

using AutoMapper;
using AutoMapper.QueryableExtensions;

using PagedList;

using CarService.Data;
using CarService.Models;
using CarService.Common;
using CarService.Web.ViewModels;

namespace CarService.Web.Controllers
{
    [Authorize(Roles=GlobalConstants.AdminRole)]
    public class EmployeesController : BaseController
    {
        public EmployeesController(ICarServiceData data)
            : base(data)
        {
        }

        public ActionResult Index(int? page, string sort)
        {
            ViewBag.Name = sort == "name_desc" ? "" : "name_desc";
            ViewBag.Username = sort == "username" ? "username_desc" : "username";
            ViewBag.DocCreated = sort == "doc_created" ? "doc_created_desc" : "doc_created";
            ViewBag.CreatedTotal = sort == "created_total" ? "created_total_desc" : "created_total";
            ViewBag.DocServed = sort == "doc_served" ? "doc_served_desc" : "doc_served";
            ViewBag.ServedTotal = sort == "served_total" ? "served_total_desc" : "served_total";

            var roles = Data.Roles.All();

            var employees = Data
                .Users
                .All()
                .Project()
                .To<EmployeeIndex>(new { roles = roles });

            switch (sort)
            {
                case "name_desc": employees = employees
                     .OrderByDescending(e => e.FirstName)
                     .ThenByDescending(e => e.LastName)
                     .ThenByDescending(e => e.UserName);
                    break;
                case "username": employees = employees
                 .OrderBy(e => e.UserName);
                    break;
                case "username_desc": employees = employees
                 .OrderByDescending(e => e.UserName);
                    break;
                case "doc_created": employees = employees
                 .OrderBy(e => e.DocumentsCreatedCount)
                 .ThenBy(e => e.UserName);
                    break;
                case "doc_created_desc": employees = employees
                 .OrderByDescending(e => e.DocumentsCreatedCount)
                 .ThenByDescending(e => e.UserName);
                    break;
                case "created_total": employees = employees
                 .OrderBy(e => e.DocumentsCreatedTotalAmount)
                 .ThenBy(e => e.UserName);
                    break;
                case "created_total_desc": employees = employees
                 .OrderByDescending(e => e.DocumentsCreatedTotalAmount)
                 .ThenByDescending(e => e.UserName);
                    break;
                case "doc_served": employees = employees
                 .OrderBy(e => e.DocumentsServedCount)
                 .ThenBy(e => e.UserName);
                    break;
                case "doc_served_desc": employees = employees
                 .OrderByDescending(e => e.DocumentsServedCount)
                 .ThenByDescending(e => e.UserName);
                    break;
                case "served_total": employees = employees
                 .OrderBy(e => e.DocumentsServedTotalAmount)
                 .ThenBy(e => e.UserName);
                    break;
                case "served_total_desc": employees = employees
                 .OrderByDescending(e => e.DocumentsServedTotalAmount)
                 .ThenByDescending(e => e.UserName);
                    break;
                default: employees = employees
                     .OrderBy(e => e.FirstName)
                     .ThenBy(e => e.LastName)
                     .ThenBy(e => e.UserName);
                    break;
            }

            var employeesModel = employees.ToPagedList(page ?? 1, 10);

            return View(employeesModel);
        }

        public ActionResult Details(string id, int? page_created, int? page_served)
        {
            if (String.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var roles = Data.Roles.All();
            int pageSizeCreated = 10;
            int pageSizeServed = 10;
            int skip_created = pageSizeCreated * (page_created.HasValue && page_created.Value > 0 ? page_created.Value - 1 : 0);
            int skip_served = pageSizeServed * (page_served.HasValue && page_served.Value > 0 ? page_served.Value - 1 : 0);
            var employee = Data
                .Users
                .All()
                .Where(u => u.Id == id)
                .Project()
                .To<EmployeeDetails>(new 
                {
                    roles = roles, 
                    skip_created = skip_created, 
                    skip_served = skip_served,
                    pageSizeCreated = pageSizeCreated,
                    pageSizeServed = pageSizeServed
                })
                .FirstOrDefault();

            if (employee == null)
            {
                return HttpNotFound();
            }

            return View(employee);
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var roles = Data.Roles.All();
            var employee = Data
                .Users
                .All()
                .Where(e => e.Id == id)
                .Project()
                .To<EmployeeEdit>(new { roles = roles})
                .FirstOrDefault();

            if (employee == null)
            {
                return HttpNotFound();
            }

            ViewBag.Roles = roles.Select(r => new SelectListItem { Value = r.Name, Text = r.Name.ToUpper(), Selected = r.Name == employee.CurrentRole }).AsEnumerable();

            return View(employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EmployeeEdit model)
        {
            if (ModelState.IsValid)
            {
                var userManager = new UserManager<User>(new UserStore<User>(Data.Context));
                var employee = userManager.FindById(model.Id);
                Mapper.Map<EmployeeEdit, User>(model, employee);
                userManager.RemoveFromRole(employee.Id, model.CurrentRole);
                userManager.AddToRole(employee.Id, model.Role);
                userManager.Update(employee);

                //var employee = Data
                //    .Users
                //    .All()
                //    .Include(u => u.Roles)
                //    .FirstOrDefault(u => u.Id == model.Id);

                //Mapper.Map<EmployeeEdit, User>(model, employee);
                //var newRole = new IdentityUserRole { RoleId = model.Role, UserId = employee.Id };
                //employee.Roles.ToList().ForEach(r => Data.Context.Entry<IdentityUserRole>(r).State = EntityState.Deleted);
                //Data.Context.Entry<IdentityUserRole>(newRole).State = EntityState.Added;
                //Data.Users.Update(employee);
                //Data.SaveChanges();
                
                return RedirectToAction("Index");
            }

            ViewBag.Roles = Data.Roles.All().Select(r => new SelectListItem { Value = r.Name, Text = r.Name.ToUpper(), Selected = r.Name == model.CurrentRole }).AsEnumerable();

            return View(model);
        }

        [HttpGet]
        public ActionResult ChangePassword()
        {
            return View(new EmployeeChangePassword());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(EmployeeChangePassword model)
        {
            if (ModelState.IsValid)
            {
                var userManager = new UserManager<User>(new UserStore<User>(Data.Context));
                if (userManager.CheckPassword(CurrentUser, model.CurrentPassword))
                {
                    var result = userManager.ChangePassword(CurrentUserId, model.CurrentPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", String.Join(", ", result.Errors));
                    }
                }
                else
                {
                    ModelState.AddModelError("CurrentPassword", "Wrong password");
                }
            }
            return View(model);
        }
    }
}