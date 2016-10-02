using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarService.Web.Infrastructure.ModelBinders
{
    public class DateTimeModelBinder:DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            object result = null;
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            if (value != null)
            {
                string valueResult = value.AttemptedValue.Trim();
                if (String.IsNullOrEmpty(valueResult))
                {
                    return base.BindModel(controllerContext, bindingContext);
                }
                try
                {
                    if (bindingContext.ModelType == typeof(DateTime) || bindingContext.ModelType == typeof(DateTime?))
                    {
                        result = DateTime.Parse(valueResult, System.Threading.Thread.CurrentThread.CurrentCulture, System.Globalization.DateTimeStyles.AllowWhiteSpaces);
                    }
                }
                catch (Exception ex)
                {
                    bindingContext.ModelState.AddModelError(bindingContext.ModelName, ex.Message);
                }
                return result;
            }
            else
            {
                return base.BindModel(controllerContext, bindingContext);
            }
        }
    }
}