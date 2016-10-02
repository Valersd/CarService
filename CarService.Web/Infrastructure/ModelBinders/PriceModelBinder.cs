using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarService.Web.Infrastructure.ModelBinders
{
    public class PriceModelBinder : DefaultModelBinder
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
                    var currencyCulture = CultureInfo.InvariantCulture;
                    var currency = controllerContext.HttpContext.Cache["currency"];
                    if (currency != null)
                    {
                        currencyCulture = CultureInfo.CreateSpecificCulture(currency.ToString());
                    }
                    if (bindingContext.ModelType == typeof(decimal) || bindingContext.ModelType == typeof(decimal?))
                    {
                        result = decimal.Parse(valueResult, NumberStyles.Currency, currencyCulture);
                    }
                }
                catch (FormatException)
                {
                    bindingContext.ModelState.AddModelError(bindingContext.ModelName, "Price is not in valid format");
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