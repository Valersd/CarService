using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Microsoft.AspNet.Identity.EntityFramework;

using AutoMapper;

using PagedList;

using CarService.Models;
using CarService.Web.Models;
using CarService.Web.ViewModels;
using CarService.Web.Infrastructure.ExtensionMethods;
using CarService.Web.Infrastructure.AutoMapperExtensions;

namespace CarService.Web.App_Start
{
    public static class AutoMapperConfig
    {
        public static void RegisterMappings()
        {
            #region RepairDocuments

            Mapper.CreateMap<RepairDocument, RepairDocumentHomeIndex>()
                .ForMember(r => r.CarRegNumber, m => m.MapFrom(s => s.Car.RegNumber));

            Mapper.CreateMap<RepairDocument, RepairDocumentDetails>()
                .ForMember(d => d.Parts, m => m.MapFrom(s => s.DocumentsParts.OrderBy(p => p.Part.Name)))
                .ForMember(d => d.PartsPrice, m => m.MapFrom(s => s.FinishedOn.HasValue ? 
                    s.DocumentsParts.Sum(dp => (decimal?)dp.UnitPrice * dp.Quantity) ?? 0m 
                    : s.DocumentsParts.Sum(dp => (decimal?)dp.Part.Price * dp.Quantity) ?? 0m));
                //.ForMember(d => d.PartsPrice, m => m.ResolveUsing<PartsPriceResolver>());
                //.ForMember(d => d.PartsPrice, m => m.MapFrom(s => s.DocumentsParts.Sum(dp => (decimal?)dp.Part.Price * dp.Quantity) ?? 0m));

            Mapper.CreateMap<RepairDocument, RepairDocumentEdit>()
                .ForMember(r => r.CarRegNumber, m => m.MapFrom(s => s.Car.RegNumber))
                .ForMember(d => d.UsedParts, m => m.MapFrom(s => s.DocumentsParts.OrderBy(p => p.Part.Name).ToList()))
                .ForMember(d => d.PartsPrice, m => m.MapFrom(s => s.FinishedOn.HasValue ? 
                    s.DocumentsParts.Sum(dp => (decimal?)dp.UnitPrice * dp.Quantity) ?? 0m 
                    : s.DocumentsParts.Sum(dp => (decimal?)dp.Part.Price * dp.Quantity) ?? 0m))
                //.ForMember(d => d.PartsPrice, m => m.MapFrom(s => s.DocumentsParts.Sum(dp => (decimal?)dp.Part.Price * dp.Quantity) ?? 0m))
                .ForMember(d => d.Categories, m => m.Ignore())
                .ForMember(d => d.Mechanics, m => m.Ignore());

            Mapper.CreateMap<RepairDocument, RepairDocumentSearch>()
                .ForMember(d => d.PartsPrice, m => m.MapFrom(s => s.FinishedOn.HasValue ?
                    s.DocumentsParts.Sum(dp => (decimal?)dp.UnitPrice * dp.Quantity) ?? 0m
                    : s.DocumentsParts.Sum(dp => (decimal?)dp.Part.Price * dp.Quantity) ?? 0m));
                //.ForMember(d => d.Parts, m => m.MapFrom(s => s.DocumentsParts))
                //.ForMember(d => d.PartsPrice, m => m.ResolveUsing<PartsPriceResolver>());
            //.ForMember(d => d.PartsPrice, m => m.MapFrom(s => s.DocumentsParts.Sum(dp => (decimal?)dp.Part.Price * dp.Quantity) ?? 0m));

            Mapper.CreateMap<RepairDocumentEdit, RepairDocument>()
                .ForMember(d => d.RepairDescription, m => m.MapFrom(s => s.RepairDescription))
                .ForMember(d => d.Mechanic, m => m.Ignore())
                .ForMember(d => d.DocumentsParts, m => m.MapFrom(s => s.UsedParts));

            Mapper.CreateMap<RepairDocument, RepairDocumentCreate>()
                .ForMember(d => d.UsedParts, m => m.MapFrom(s => s.DocumentsParts))
                .ForMember(d => d.Categories, m => m.Ignore())
                .ForMember(d => d.Mechanics, m => m.Ignore())
                .ForMember(d => d.CarId, m => m.Ignore());

            Mapper.CreateMap<RepairDocumentCreate, RepairDocument>()
                .ForMember(r => r.CarId, m => m.MapFrom(s => s.CarId.Value))
                .ForMember(r => r.MechanicId, m => m.MapFrom(s => s.MechanicId))
                .ForMember(r => r.DocumentsParts, m => m.MapFrom(s => s.UsedParts))
                .ForMember(r => r.Mechanic, m => m.Ignore());

            #endregion

            #region User

            Mapper.CreateMap<User, Employee>();

            IQueryable<IdentityRole> roles = null;
            Mapper.CreateMap<User, EmployeeIndex>()
                .ForMember(e => e.DocumentsCreatedCount, m => m.MapFrom(s => s.CreatedRepairDocuments.Count))
                .ForMember(e => e.DocumentsCreatedTotalAmount, m => m.MapFrom(s => (decimal?)s.CreatedRepairDocuments.Sum(d => d.TotalPrice) ?? 0m))
                .ForMember(e => e.DocumentsServedCount, m => m.MapFrom(s => s.ServedRepairDocuments.Count))
                .ForMember(e => e.DocumentsServedTotalAmount, m => m.MapFrom(s => (decimal?)s.ServedRepairDocuments.Sum(d => d.TotalPrice) ?? 0m))
                .ForMember(e => e.Role, m => m.MapFrom(s => s.Roles.Join(roles, sr => sr.RoleId, r => r.Id, (sr, r) => r.Name).FirstOrDefault()));

            int skip_created = 0;
            int pageSizeCreated = 0;
            int skip_served = 0;
            int pageSizeServed = 0;
            Mapper.CreateMap<User, EmployeeDetails>()
                .ForMember(e => e.DocumentsCreatedCount, m => m.MapFrom(s => s.CreatedRepairDocuments.Count))
                .ForMember(e => e.DocumentsCreatedTotalAmount, m => m.MapFrom(s => (decimal?)s.CreatedRepairDocuments.Sum(d => d.TotalPrice) ?? 0m))
                .ForMember(e => e.DocumentsServedCount, m => m.MapFrom(s => s.ServedRepairDocuments.Count))
                .ForMember(e => e.DocumentsServedTotalAmount, m => m.MapFrom(s => (decimal?)s.ServedRepairDocuments.Sum(d => d.TotalPrice) ?? 0m))
                .ForMember(e => e.Role, m => m.MapFrom(s => s.Roles.Join(roles, sr => sr.RoleId, r => r.Id, (sr, r) => r.Name).FirstOrDefault()))
                .ForMember(e => e.CreatedRepairDocuments, m => m.MapFrom(s=>s.CreatedRepairDocuments.OrderByDescending(d=>d.CreatedOn).Skip(skip_created).Take(pageSizeCreated)))
                .ForMember(e => e.ServedRepairDocuments, m => m.MapFrom(s=>s.ServedRepairDocuments.OrderByDescending(d=>d.CreatedOn).Skip(skip_served).Take(pageSizeServed)));

            Mapper.CreateMap<User, RegisterViewModel>()
                .ForMember(e => e.Role, m => m.Ignore());

            Mapper.CreateMap<RegisterViewModel, User>()
                .ForMember(e => e.FirstName, m => m.MapFrom(s => s.FirstName.ToUpper()))
                .ForMember(e => e.LastName, m => m.MapFrom(s => s.LastName.ToUpper()))
                .ForMember(e => e.UserName, m => m.MapFrom(s => s.UserName.ToLower()));

            Mapper.CreateMap<User, EmployeeEdit>()
                .ForMember(e => e.Role, m => m.Ignore())
                .ForMember(e => e.CurrentRole, m => m.MapFrom(s => s.Roles.Join(roles, sr=>sr.RoleId, r=>r.Id, (sr, r) => r.Name).FirstOrDefault()));

            Mapper.CreateMap<EmployeeEdit, User>()
                .ForMember(e => e.FirstName, m => m.MapFrom(s => s.FirstName.ToUpper()))
                .ForMember(e => e.LastName, m => m.MapFrom(s => s.LastName.ToUpper()))
                .ForMember(e => e.UserName, m => m.MapFrom(s => s.UserName.ToLower()));

            #endregion

            #region Car

            Mapper.CreateMap<Car, CarInRepairDocumentDetails>();

            Mapper.CreateMap<Car, CarEdit>();

            Mapper.CreateMap<CarEdit, Car>()
                .ForMember(c => c.Vendor, m => m.MapFrom(s => s.Vendor.ToUpperInvariant()))
                .ForMember(c => c.Model, m => m.MapFrom(s => s.Model.ToUpperInvariant()));

            Mapper.CreateMap<Car, CarCreate>();

            Mapper.CreateMap<CarCreate, Car>()
                .ForMember(c => c.Vendor, m => m.MapFrom(s => s.Vendor.ToUpperInvariant()))
                .ForMember(c => c.Model, m => m.MapFrom(s => s.Model.ToUpperInvariant()))
                .ForMember(c => c.RegNumber, m => m.MapFrom(s => s.RegNumber.CyrillicToLatinToUpper()))
                .ForMember(c => c.ChassisNumber, m => m.MapFrom(s => s.ChassisNumber.ToUpperInvariant()))
                .ForMember(c => c.EngineNumber, m => m.MapFrom(s => s.EngineNumber.ToUpperInvariant()));

            Mapper.CreateMap<Car, CarSearch>()
                .ForMember(c => c.RepairsCount, m => m.MapFrom(s => s.RepairDocuments.Count))
                .ForMember(c => c.PartsPrice, m => m.MapFrom(s => s.RepairDocuments
                    .Sum(d => (decimal?)d.DocumentsParts
                        .Sum(dp => (decimal?)dp.UnitPrice == null ? dp.Part.Price * dp.Quantity : dp.UnitPrice * dp.Quantity ?? 0m)) ?? 0m))
                .ForMember(c => c.TotalPrice, m => m.MapFrom(s => s.RepairDocuments.Sum(d => d.TotalPrice)))
                .ForMember(c => c.LastRepairDateTime, m => m.MapFrom(s => s.RepairDocuments.Max(d => d.CreatedOn)));

            Mapper.CreateMap<Car, CarDetails>()
                .ForMember(c => c.RepairDocuments, m => m.MapFrom(s => s.RepairDocuments));

            #endregion

            #region ReplacementPart

            Mapper.CreateMap<ReplacementPart, PartInRepairDocumentDetails>()
                .ForMember(p => p.Category, m => m.MapFrom(s => s.Category.Name))
                .ForMember(p => p.Quantity, m => m.Ignore());

            Mapper.CreateMap<DocumentsParts, PartInRepairDocumentDetails>()
                .ForMember(p => p.CatalogNumber, m => m.MapFrom(s => s.Part.CatalogNumber))
                .ForMember(p => p.Category, m => m.MapFrom(s => s.Part.Category.Name))
                .ForMember(p => p.Name, m => m.MapFrom(s => s.Part.Name))
                .ForMember(p => p.Price, m => m.MapFrom(s => s.UnitPrice.HasValue ? s.UnitPrice.Value : s.Part.Price))
                .ForMember(p => p.Id, m => m.MapFrom(s => s.PartId))
                .ForMember(p => p.IsActive, m => m.MapFrom(s => s.Part.IsActive))
                .ForMember(p => p.Quantity, m => m.MapFrom(s => s.Quantity));

            Mapper.CreateMap<PartInRepairDocumentDetails, DocumentsParts>()
                .ForMember(p => p.Quantity, m => m.MapFrom(s => s.Quantity))
                .ForMember(p => p.PartId, m => m.MapFrom(s => s.Id));

            Mapper.CreateMap<ReplacementPart, PartSearch>()
                .ForMember(p => p.CurrentPrice, m => m.MapFrom(s => (decimal?)s.Price ?? 0m))
                .ForMember(p => p.Category, m => m.MapFrom(s => s.Category.Name))
                .ForMember(p => p.TotalUsedNumber, m => m.MapFrom(s => (int?)s.DocumentsParts.Sum(dp => dp.Quantity) ?? 0))
                .ForMember(p => p.TotalAmount, m => m.MapFrom(s => (decimal?)s.DocumentsParts.Sum(dp => dp.UnitPrice == null ? dp.Quantity * s.Price : dp.Quantity * dp.UnitPrice ?? 0m) ?? 0m));

            Mapper.CreateMap<ReplacementPart, PartInCategoryDetails>()
                .ForMember(p => p.CurrentPrice, m => m.MapFrom(s => (decimal?)s.Price ?? 0m))
                .ForMember(p => p.TotalUsedNumber, m => m.MapFrom(s => (int?)s.DocumentsParts.Sum(dp => dp.Quantity) ?? 0))
                .ForMember(p => p.TotalAmount, m => m.MapFrom(s => (decimal?)s.DocumentsParts.Sum(dp => dp.UnitPrice == null ? dp.Quantity * s.Price : dp.Quantity * dp.UnitPrice ?? 0m) ?? 0m));

            Mapper.CreateMap<ReplacementPart, PartBase>();

            Mapper.CreateMap<PartBase, ReplacementPart>()
                .ForMember(p => p.CatalogNumber, m => m.MapFrom(s => s.CatalogNumber.ToUpperInvariant()))
                .ForMember(p => p.Name, m => m.MapFrom(s => s.Name.ToUpperInvariant()));

            Mapper.CreateMap<ReplacementPart, PartEdit>();

            Mapper.CreateMap<PartEdit, ReplacementPart>()
                .ForMember(p => p.CatalogNumber, m => m.MapFrom(s => s.CatalogNumber.ToUpperInvariant()))
                .ForMember(p => p.Name, m => m.MapFrom(s => s.Name.ToUpperInvariant()));

            #endregion

            #region Category

            Mapper.CreateMap<Category, CategoryBase>();
            Mapper.CreateMap<CategoryBase, Category>()
                .ForMember(c=>c.Name, m=>m.MapFrom(s=>s.Name.ToUpperInvariant()));

            Mapper.CreateMap<Category, CategoryIndex>();
            Mapper.CreateMap<CategoryIndex, Category>()
                .ForMember(c => c.Name, m => m.MapFrom(s => s.Name.ToUpperInvariant()));

            Mapper.CreateMap<Category, CategoryIndexExtended>()
                .ForMember(c => c.Parts, m => m.Ignore())
                .ForMember(c => c.PartsCount, m => m.MapFrom(s => s.ReplacementParts.Count))
                .ForMember(c => c.TotalUsedPartsNumber, m => m.MapFrom(s => (int?)s.ReplacementParts.Sum(p => (int?)p.DocumentsParts.Sum(dp => dp.Quantity) ?? 0) ?? 0))
                .ForMember(c => c.TotalUsedPartsAmount, m => m.MapFrom(s => (decimal?)s.ReplacementParts.Sum(p => (decimal?)p.DocumentsParts.Sum(dp => dp.UnitPrice == null ? p.Price * dp.Quantity : dp.UnitPrice * dp.Quantity) ?? 0m) ?? 0m));

            #endregion
        }
    }
}