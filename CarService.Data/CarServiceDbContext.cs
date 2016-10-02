using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Data.Entity.Infrastructure;

using System.Collections.Generic;

using Microsoft.AspNet.Identity.EntityFramework;

using CarService.Models;

namespace CarService.Data
{
    public class CarServiceDbContext : IdentityDbContext<User>
    {
        public CarServiceDbContext()
            : base("CarService", throwIfV1Schema: false)
        {
            this.Configuration.LazyLoadingEnabled = false;
        }

        public virtual IDbSet<Car> Cars { get; set; }

        public virtual IDbSet<RepairDocument> RepairDocuments { get; set; }

        public virtual IDbSet<Category> Categories { get; set; }

        public virtual IDbSet<ReplacementPart> ReplacementParts { get; set; }

        public virtual IDbSet<DocumentsParts> DocumentsParts { get; set; }


        public static CarServiceDbContext Create()
        {
            return new CarServiceDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder
                .Entity<RepairDocument>()
                .HasRequired(d => d.CreatedBy)
                .WithMany(u => u.CreatedRepairDocuments)
                .HasForeignKey(d=>d.CreatedById)
                .WillCascadeOnDelete(false);

            modelBuilder
                .Entity<RepairDocument>()
                .HasRequired(d => d.Mechanic)
                .WithMany(u => u.ServedRepairDocuments)
                .HasForeignKey(d => d.MechanicId)
                .WillCascadeOnDelete(false);

            //modelBuilder
            //    .Entity<RepairDocument>()
            //    .HasMany(d => d.ReplacementParts)
            //    .WithMany(r => r.RepairDocuments)
            //    .Map(m =>
            //    {
            //        m.MapLeftKey("RepairDocumentId");
            //        m.MapRightKey("ReplacementPartId");
            //        m.ToTable("ReplacementPartRepairDocuments");
            //    });
        }

        protected override DbEntityValidationResult ValidateEntity(DbEntityEntry entityEntry, IDictionary<object, object> items)
        {
            var currentResult = base.ValidateEntity(entityEntry, items);
            if (entityEntry.State == EntityState.Modified)
            {
                var errors = currentResult.ValidationErrors;
                if (errors.Count > 0)
                {
                    List<DbValidationError> result = new List<DbValidationError>();
                    foreach (var error in errors)
                    {
                        string propName = error.PropertyName;
                        if (entityEntry.Property(propName).IsModified)
                        {
                            result.Add(error);
                        }
                    }
                    return new DbEntityValidationResult(entityEntry, result);
                }
            }
            return currentResult;
        }
    }
}
