using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using AutoMapper;

using CarService.Models;

namespace CarService.Web.Infrastructure.AutoMapperExtensions
{
    public class PartsPriceResolver : ValueResolver<RepairDocument, decimal?>
    {
        protected override decimal? ResolveCore(RepairDocument source)
        {
            if (!source.DocumentsParts.Any())
            {
                return 0m;
            }
            bool isFinished = source.FinishedOn.HasValue;
            if (isFinished)
            {
                return source.DocumentsParts.Sum(dp => dp.UnitPrice * dp.Quantity);
            }
            else
            {
                return source.DocumentsParts.Sum(dp => dp.Part.Price * dp.Quantity);
            }
        }
    }
}