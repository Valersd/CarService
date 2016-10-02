using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using CarService.Web.Infrastructure.ModelBinders;

namespace CarService.Web.App_Start
{
    public static class ModelBindersConfig
    {
        public static void RegisterBindings()
        {
            ModelBinders.Binders.Add(typeof(decimal), new PriceModelBinder());
            ModelBinders.Binders.Add(typeof(decimal?), new PriceModelBinder());

            ModelBinders.Binders.Add(typeof(DateTime), new DateTimeModelBinder());
            ModelBinders.Binders.Add(typeof(DateTime?), new DateTimeModelBinder());
        }
    }
}