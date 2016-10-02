using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Microsoft.AspNet.Identity;

using System.Data.Entity.Infrastructure;

using CarService.Data;
using CarService.Models;
using CarService.Common;

namespace CarService.Web.Controllers
{
    [Authorize(Roles="Admin, Employee")]
    public class BaseController : Controller
    {
        private readonly ICarServiceData _data;

        public BaseController(ICarServiceData data)
        {
            _data = data;
        }

        public ICarServiceData Data
        {
            get { return _data; }
        }

        public User CurrentUser
        {
            get { return User.Identity.IsAuthenticated ? _data.Users.GetById(User.Identity.GetUserId()) : default(User); }
        }

        public string CurrentUserId
        {
            get { return User.Identity.IsAuthenticated ? User.Identity.GetUserId() : default(string); }
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (HttpContext.Request.Cookies.AllKeys.Contains("timezoneoffset"))
            {
                Session["timezoneoffset"] =
                    HttpContext.Request.Cookies["timezoneoffset"].Value;
            }

            //var languages = HttpContext.Request.UserLanguages;
            //if (languages != null && languages.Length > 0)
            //{
            //    System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture(languages[0].Trim());
            //}
            //else
            //{
            //    System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
            //}
            string currency = "bg-BG";
            HttpContext.Cache["currency"] = currency;

            base.OnActionExecuting(filterContext);
        }

        protected virtual void HandleDbUpdateException(DbUpdateException ex)
        {
            string message = ex.InnerException.InnerException.Message;
            bool uniqueConstraint = false;
            foreach (var e in ex.Entries)
            {
                var properties = e.CurrentValues.PropertyNames;
                foreach (var prop in properties)
                {
                    if (message.IndexOf(prop, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        uniqueConstraint = true;
                        ModelState.AddModelError(prop, string.Format("{0} already exists", e.CurrentValues.GetValue<string>(prop)));
                    }
                }
            }
            if (!uniqueConstraint)
            {
                ModelState.AddModelError("", "Possible duplicated value");
            }
        }
    }
}