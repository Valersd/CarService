namespace CarService.Data.Migrations
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    using Faker;
    using Faker.Extensions;

    using CarService.Models;
    using CarService.Common;

    public sealed class Configuration : DbMigrationsConfiguration<CarServiceDbContext>
    {
        private readonly List<DateTime> _startDates;
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            _startDates = GetStartDates();
        }

        protected override void Seed(CarServiceDbContext context)
        {
            SeedUsersAndRoles(context);
            SeedCars(context);
            SeedCategories(context);
            SeedReplacementParts(context);
            SeedRepairDocuments(context);
            SeedDocumentsParts(context);
        }

        private void SeedDocumentsParts(CarServiceDbContext context)
        {
            if (context.DocumentsParts.Any())
            {
                return;
            }
            var documents = context.RepairDocuments.ToList();
            var parts = context.ReplacementParts.ToList();

            foreach (var doc in documents)
            {
                var partsCount = RandomGenerator.GetRandomInteger(0, 11);
                var partsPrice = 0m;
                for (int i = 0; i < partsCount; i++)
                {
                    var part = parts[RandomGenerator.GetRandomInteger(0, parts.Count - 1)];
                    var docPart = doc.DocumentsParts.FirstOrDefault(dp => dp.PartId == part.Id);
                    if (docPart == null)
                    {
                        docPart = new DocumentsParts { DocumentId = doc.Id, PartId = part.Id };
                        if (doc.FinishedOn.HasValue)
                        {
                            docPart.UnitPrice = part.Price;
                        }
                        context.DocumentsParts.Add(docPart);
                    }
                    else
                    {
                        docPart.Quantity++;
                    }
                    partsPrice += part.Price;
                }

                doc.TotalPrice = Math.Round(partsPrice + partsPrice / 5m, 2);
                if (partsPrice == 0m)
                {
                    doc.TotalPrice = RandomGenerator.GetRandomInteger(20, 100) + 0.01m * RandomGenerator.GetRandomInteger(0, 99);
                }

                if (doc.FinishedOn == null && RandomGenerator.GetRandomInteger() % 2 == 0)
                {
                    doc.TotalPrice = null;
                }
            }
            context.SaveChanges();
        }

        private void SeedRepairDocuments(CarServiceDbContext context)
        {
            if (context.RepairDocuments.Any())
            {
                return;
            }
            var employeeIds = context.Users.Select(e => e.Id).ToList();
            var testUserId = context.Users.FirstOrDefault(u => u.UserName == "testuser").Id;
            var testAdminId = context.Users.FirstOrDefault(u => u.UserName == "testadmin").Id; 
            var carIds = context.Cars.Select(c=>c.Id).ToList();

            for (int i = 0; i < GlobalConstants.TestReapairDocumentsCount; i++)
            {
                var partsCount = RandomGenerator.GetRandomInteger(0, 11);
                var repairDoc = new RepairDocument
                {
                    CarId = carIds[RandomGenerator.GetRandomInteger(0, carIds.Count - 1)],
                    CreatedById = employeeIds[RandomGenerator.GetRandomInteger(0, employeeIds.Count - 1)],
                    MechanicId = employeeIds[RandomGenerator.GetRandomInteger(0, employeeIds.Count - 1)],
                    RepairDescription = String.Join(Environment.NewLine, Lorem.Paragraphs(RandomGenerator.GetRandomInteger(1, 2))),
                };

                repairDoc.CreatedOn = _startDates[i];

                repairDoc.FinishedOn = GetFinishedDate(repairDoc.CreatedOn);
                if (!repairDoc.FinishedOn.HasValue  && RandomGenerator.GetRandomInteger() > 50)
                {
                    repairDoc.CreatedById = RandomGenerator.GetRandomInteger() > 33 ? testAdminId : testUserId;
                }

                context.RepairDocuments.Add(repairDoc);
            }
            context.SaveChanges();
            var cars = context.Cars.Where(c => !c.RepairDocuments.Any()).ToList();
            foreach (var car in cars)
            {
                context.Cars.Remove(car);
            }
            context.SaveChanges();
        }

        private void SeedReplacementParts(CarServiceDbContext context)
        {
            if (context.ReplacementParts.Any())
            {
                return;
            }
            var categories = context.Categories.ToList();

            for (int i = 0; i < GlobalConstants.TestReplacementPartsCount; i++)
            {
                Category category = categories[RandomGenerator.GetRandomInteger(0, categories.Count - 1)];
                string categoryIdentifier = category.Name.Substring(0, 4).Replace(' ', 'X');
                string index = i.ToString().PadLeft(3, '0');
                ReplacementPart part = new ReplacementPart
                {
                    Name = String.Join(" ", Lorem.Words(RandomGenerator.GetRandomInteger(2, 4))).ToUpper(),
                    CategoryId = category.Id,
                    CatalogNumber = categoryIdentifier + index + RandomGenerator.GetRandomInteger(0, 999).ToString().PadLeft(3, '0'),
                    Price = (decimal)RandomGenerator.GetRandomInteger(5, 400) + 0.01m * (decimal)RandomGenerator.GetRandomInteger(0, 99),
                    IsActive = RandomGenerator.GetRandomInteger() < 75
                };
                context.ReplacementParts.Add(part);
            }
            context.SaveChanges();
        }

        private void SeedCategories(CarServiceDbContext context)
        {
            if (context.Categories.Any())
            {
                return;
            }
            for (int i = 0; i < GlobalConstants.TestCategoriesCount; i++)
            {
                Category category = new Category
                {
                    Name = String.Join(" ", Lorem.Words(RandomGenerator.GetRandomInteger(2, 4))).ToUpper()
                };
                context.Categories.Add(category);
            }
            context.SaveChanges();
        }

        private void SeedCars(CarServiceDbContext context)
        {
            if (context.Cars.Any())
            {
                return;
            }

            string[] vendors = new string[] { "Peugeot", "Renault", "Ford", "Opel", "BMW", "Dacia", "Toyota", "Mazda", "Seat", "Saab", "Mercedes", "Alfa Romeo", "Volkswagen"};
            string[] regNumbers = new string[] { "C", "CA", "CB", "PK", "KH", "CO", "E", "C", "CA", "CB" };
            string[] lastLetters = new string[] { "A", "B", "C", "E", "H", "K", "M", "O", "P", "T", "X" };
            string[] colors = new string[] { "red", "black", "blue", "white", "silver", "green", "yellow" };
            int[] engines = new int[] { 1400, 1600, 1800, 2000, 2200, 2400, 2600, 2800 };
            string[] phoneCodes = new string[] { "0888", "0878", "0898" };

            for (int i = 0; i < GlobalConstants.TestCarsCount; i++)
            {
                Car car = new Car
                {
                    RegNumber = regNumbers[RandomGenerator.GetRandomInteger(0, regNumbers.Length - 1)] + RandomGenerator.GetRandomInteger(1, 9999).ToString().PadLeft(4, '0') + lastLetters[RandomGenerator.GetRandomInteger(0, lastLetters.Length - 1)] + lastLetters[RandomGenerator.GetRandomInteger(0, lastLetters.Length - 1)],
                    OwnerName = RandomGenerator.GetRandomInteger() % 2 == 0 ? Name.FullName(NameFormats.Standard) : null,
                    OwnerPhone = RandomGenerator.GetRandomInteger() % 2 == 0 ? phoneCodes[RandomGenerator.GetRandomInteger(0, phoneCodes.Length - 1)] + RandomGenerator.GetRandomInteger(200000, 999999) : null,
                    Year = RandomGenerator.GetRandomInteger() % 2 == 0 ? RandomGenerator.GetRandomInteger(DateTime.Now.Year - 25, DateTime.Now.Year - 1) : default(int?),
                    Color = RandomGenerator.GetRandomInteger() % 2 == 0 ? colors[RandomGenerator.GetRandomInteger(0, colors.Length - 1)] : null,
                    ChassisNumber = RandomGenerator.GetRandomString(6, 6).ToUpper() + RandomGenerator.GetRandomInteger(1000, 9999) + RandomGenerator.GetRandomString(3, 3).ToUpper() + RandomGenerator.GetRandomInteger(0, 9) + RandomGenerator.GetRandomString(3, 3).ToUpper(),
                    EngineNumber = RandomGenerator.GetRandomString(4, 4).ToUpper() + RandomGenerator.GetRandomInteger(10000, 99999) + RandomGenerator.GetRandomString(4, 4).ToUpper() + RandomGenerator.GetRandomInteger(10, 99) + RandomGenerator.GetRandomString(1, 1).ToUpper(),
                    EngineCapacity = RandomGenerator.GetRandomInteger() % 2 == 0 ? engines[RandomGenerator.GetRandomInteger(0, engines.Length - 1)] : default(int?),
                    Description = RandomGenerator.GetRandomInteger() % 2 == 0 ? Faker.Lorem.Paragraph(2) : null,
                };
                if (RandomGenerator.GetRandomInteger() % 2 == 0)
                {
                    car.Vendor = vendors[RandomGenerator.GetRandomInteger(0, vendors.Length - 1)].ToUpper();
                    car.Model = RandomGenerator.GetRandomString(3, 6).ToUpper();
                }
                context.Cars.Add(car);
            }
            context.SaveChanges();
        }

        private static void SeedUsersAndRoles(CarServiceDbContext context)
        {
            if (context.Roles.Any())
            {
                return;
            }

            context.Roles.Add(new IdentityRole(GlobalConstants.AdminRole));
            context.Roles.Add(new IdentityRole(GlobalConstants.EmployeeRole));
            context.Roles.Add(new IdentityRole(GlobalConstants.InactivRole));
            context.SaveChanges();

            var userManager = new UserManager<User>(new UserStore<User>(context));

            string[] phoneCodes = new string[] { "0888", "0878", "0898" };

            for (int i = 0; i < GlobalConstants.TestUsersCount - 2; i++)
            {
                User user = new User
                {
                    FirstName = Name.First().ToUpper(),
                    LastName = Name.Last().ToUpper(),
                    PhoneNumber = phoneCodes[RandomGenerator.GetRandomInteger(0, phoneCodes.Length - 1)] + RandomGenerator.GetRandomInteger(200000, 999999)
                };
                user.UserName = (user.FirstName + user.LastName).ToLower();
                userManager.Create(user, GlobalConstants.TestPassword);
                userManager.AddToRole(user.Id, GlobalConstants.EmployeeRole);
                context.SaveChanges();
            }

            User testadmin = new User
            {
                FirstName = "TEST",
                LastName = "ADMIN",
                UserName = "testadmin",
                PhoneNumber = phoneCodes[RandomGenerator.GetRandomInteger(0, phoneCodes.Length - 1)] + RandomGenerator.GetRandomInteger(200000, 999999)
            };
            userManager.Create(testadmin, GlobalConstants.TestPassword);
            userManager.AddToRole(testadmin.Id, GlobalConstants.AdminRole);
            context.SaveChanges();

            User testuser = new User
            {
                FirstName = "TEST",
                LastName = "USER",
                UserName = "testuser",
                PhoneNumber = phoneCodes[RandomGenerator.GetRandomInteger(0, phoneCodes.Length - 1)] + RandomGenerator.GetRandomInteger(200000, 999999)
            };
            userManager.Create(testuser, GlobalConstants.TestPassword);
            userManager.AddToRole(testuser.Id, GlobalConstants.EmployeeRole);
            context.SaveChanges();
        }

        private List<DateTime> GetStartDates()
        {
            List<DateTime> datesOnCreation = new List<DateTime>();
            DateTime startDate = DateTime.Now.AddMonths(-3);
            for (int i = 0; i < GlobalConstants.TestReapairDocumentsCount; i++)
            {
                DateTime dateOnCreation;
                do
                {
                    dateOnCreation = RandomGenerator.GetRandomDateTime(startDate, 3 * 30 * 24 + 24);
                } while (!(dateOnCreation.Hour >= 9 && dateOnCreation.Hour <= 18 && dateOnCreation.DayOfWeek != DayOfWeek.Sunday));
                datesOnCreation.Add(dateOnCreation);
            }
            datesOnCreation.Sort();
            return datesOnCreation;
        }

        private DateTime? GetFinishedDate(DateTime startDate)
        {
            DateTime finishedDate;
            do
            {
                finishedDate = RandomGenerator.GetRandomDateTime(startDate, 6 * 24);
                if (finishedDate >= DateTime.Now)
                {
                    return null;
                }
            } while (!(finishedDate.Hour >= 9 && finishedDate.Hour <= 18 && finishedDate.DayOfWeek != DayOfWeek.Sunday));
            return finishedDate;
        }
    }
}
