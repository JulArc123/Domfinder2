using DomFinder2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DomFinder2.Data
{
    public static class DataSeeder
    {
        public static async Task SeedData(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

            // Seed roles
            await SeedRoles(roleManager);

            // Seed users
            var adminUser = await SeedUser(userManager, "admin@example.com", "Admin@123");
            var testUser = await SeedUser(userManager, "test@mail.com", "Test@123");

            // Seed properties
            await SeedProperties(context, adminUser, "admin@example.com");
            await SeedProperties(context, testUser, "test@mail.com");
        }

        private static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            string[] roleNames = { "Admin" };
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }

        private static async Task<IdentityUser> SeedUser(UserManager<IdentityUser> userManager, string email, string password)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                user = new IdentityUser { UserName = email, Email = email };
                var result = await userManager.CreateAsync(user, password);
                if (result.Succeeded && email == "admin@example.com")
                {
                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }
            return user;
        }

        private static async Task SeedProperties(ApplicationDbContext context, IdentityUser user, string email)
        {
            var properties = new List<Property>();

            if (email == "admin@example.com")
            {
                properties.AddRange(new List<Property>
                {
                    new Property
                    {
                        Title = "Piękne mieszkanie w centrum",
                        Price = 1000000,
                        Area = 120,
                        Rooms = 3,
                        Description = "Piękne mieszkanie w centrum miasta z widokiem na park.",
                        City = "Warszawa",
                        District = "Mokotów",
                        Street = "ul. Koszykowa",
                        BuildingType = "Apartament",
                        Floor = 2,
                        TotalFloors = 10,
                        YearBuilt = 2015,
                        OwnershipType = "Własność",
                        AvailableFrom = DateTime.Now,
                        Rent = 3000,
                        AdvertiserType = "Właściciel",
                        EnergyCertificate = "A",
                        Internet = true,
                        CableTV = true,
                        Phone = true,
                        AntiBurglaryBlinds = true,
                        Monitoring = true,
                        AlarmSystem = true,
                        Intercom = true,
                        ClosedArea = true,
                        Furnished = true,
                        WashingMachine = true,
                        Fridge = true,
                        Stove = true,
                        Television = true,
                        Dishwasher = true,
                        Oven = true,
                        UserId = user.Id,
                        ImagePaths = new List<string> { "/images/1.webp", "/images/2.webp", "/images/3.webp" }
                    },
                    new Property
                    {
                        Title = "Luksusowy apartament z widokiem",
                        Price = 12000,
                        Area = 150,
                        Rooms = 4,
                        Description = "Luksusowy apartament z pięknym widokiem na miasto.",
                        City = "Kraków",
                        District = "Kazimierz",
                        Street = "ul. Szeroka",
                        BuildingType = "Apartament",
                        Floor = 5,
                        TotalFloors = 12,
                        YearBuilt = 2018,
                        OwnershipType = "Własność",
                        AvailableFrom = DateTime.Now,
                        Rent = 4000,
                        AdvertiserType = "Właściciel",
                        EnergyCertificate = "B",
                        Internet = true,
                        CableTV = true,
                        Phone = true,
                        AntiBurglaryBlinds = true,
                        Monitoring = true,
                        AlarmSystem = true,
                        Intercom = true,
                        ClosedArea = true,
                        Furnished = true,
                        WashingMachine = true,
                        Fridge = true,
                        Stove = true,
                        Television = true,
                        Dishwasher = true,
                        Oven = true,
                        UserId = user.Id,
                        ImagePaths = new List<string> { "/images/4.webp", "/images/5.webp", "/images/6.webp" }
                    }
                });
            }

            if (email == "test@mail.com")
            {
                properties.AddRange(new List<Property>
                {
                    new Property
                    {
                        Title = "Urokliwy domek na przedmieściach",
                        Price = 750000,
                        Area = 200,
                        Rooms = 5,
                        Description = "Przestronny dom z ogrodem na spokojnym osiedlu.",
                        City = "Gdańsk",
                        District = "Oliwa",
                        Street = "ul. Piękna",
                        BuildingType = "Dom",
                        Floor = 2,
                        TotalFloors = 2,
                        YearBuilt = 2010,
                        OwnershipType = "Własność",
                        AvailableFrom = DateTime.Now,
                        Rent = 5000,
                        AdvertiserType = "Właściciel",
                        EnergyCertificate = "C",
                        Internet = true,
                        CableTV = true,
                        Phone = true,
                        AntiBurglaryBlinds = true,
                        Monitoring = true,
                        AlarmSystem = true,
                        Intercom = true,
                        ClosedArea = true,
                        Furnished = true,
                        WashingMachine = true,
                        Fridge = true,
                        Stove = true,
                        Television = true,
                        Dishwasher = true,
                        Oven = true,
                        UserId = user.Id,
                        ImagePaths = new List<string> { "/images/7.webp", "/images/8.webp", "/images/9.webp" }
                    },
                    new Property
                    {
                        Title = "Nowoczesny apartament w wieżowcu",
                        Price = 4000,
                        Area = 100,
                        Rooms = 3,
                        Description = "Stylowy apartament z widokiem na morze.",
                        City = "Poznań",
                        District = "Centrum",
                        Street = "ul. Morska",
                        BuildingType = "Apartament",
                        Floor = 10,
                        TotalFloors = 15,
                        YearBuilt = 2020,
                        OwnershipType = "Własność",
                        AvailableFrom = DateTime.Now,
                        Rent = 3500,
                        AdvertiserType = "Właściciel",
                        EnergyCertificate = "A",
                        Internet = true,
                        CableTV = true,
                        Phone = true,
                        AntiBurglaryBlinds = true,
                        Monitoring = true,
                        AlarmSystem = true,
                        Intercom = true,
                        ClosedArea = true,
                        Furnished = true,
                        WashingMachine = true,
                        Fridge = true,
                        Stove = true,
                        Television = true,
                        Dishwasher = true,
                        Oven = true,
                        UserId = user.Id,
                        ImagePaths = new List<string> { "/images/10.webp", "/images/11.webp" }
                    }
                });
            }

            context.Properties.AddRange(properties);
            await context.SaveChangesAsync();
        }
    }
}
