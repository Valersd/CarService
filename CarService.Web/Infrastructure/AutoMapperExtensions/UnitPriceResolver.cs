using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using AutoMapper;

using CarService.Models;

namespace CarService.Web.Infrastructure.AutoMapperExtensions
{
    public class UnitPriceResolver : ValueResolver<DocumentsParts, decimal?>
    {
        protected override decimal? ResolveCore(DocumentsParts source)
        {
            bool isFinished = source.Document.FinishedOn.HasValue;
            if (isFinished)
            {
                return source.UnitPrice;
            }
            else
            {
                return source.Part.Price;
            }
        }
    }
}