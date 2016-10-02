using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

using Microsoft.AspNet.Identity.EntityFramework;

using CarService.Models;

namespace CarService.Data
{
    public interface ICarServiceData
    {
        IRepository<Category> Categories { get; }
        IRepository<ReplacementPart> ReplacementParts { get; }
        IRepository<RepairDocument> RepairDocuments { get; }
        IRepository<DocumentsParts> DocumentsParts { get; }
        IRepository<Car> Cars { get; }
        IRepository<User> Users { get; }
        IRepository<IdentityRole> Roles { get; }
        DbContext Context { get; }
        void Dispose();
        int SaveChanges();
    }
}
