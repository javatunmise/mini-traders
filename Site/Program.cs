using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Site.Data;

namespace Site
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args)
                .Build()
                .SeedData(SeedLocations)
                .Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();


        private static void SeedLocations(ApplicationDbContext dbContext)
        {
            var existingCampus = dbContext.Campuses.ToList();
            var locationNames = new[] { "Unilag" , "Yabatech" , "FCE AKOKA" , "Lagos State University" };
            foreach(var location in locationNames)
            {
                if (existingCampus.Any(e => e.Name.ToLower() == location.ToLower()))
                    continue;

                dbContext.Campuses.Add(new Shared.Entities.Campus { Name = location });
            }

            dbContext.SaveChanges();

            var unilag = dbContext.Campuses.Single(e => e.Name == locationNames[0]);
            var yabatech = dbContext.Campuses.Single(e => e.Name == locationNames[1]);
            var fceAkoka = dbContext.Campuses.Single(e => e.Name == locationNames[2]);
            var lasu = dbContext.Campuses.Single(e => e.Name == locationNames[3]);

            var unilagHostels = new[] {"Moremi ","Abimbola Awoliyi ","Samuel Manuwa ","Makama ","MTH ","Fagunwa ","Queen Amina ","Kofo Ademola ","Honours ","Women Society ","JAJA ","Mariere ","Eni Njoku ","Sodeinde ","Biobaku ","Bariga ","Shomolu ","Abule Oja ","Onike ","Ladylak ","Pako ","Chemist ","Off campus" };
            var yabatechHostels = new[] {"Hollywood ","Bakasy ","Akata ","Complex ","PGD ","New female hostel ","Igbobi sabo ","Onike ","Ladylak ","Pako ","Shomolu ","Chemist ","Bariga ","Off campus" };
            var fceAkokaHostels = new[] { "Hall 1 ", "Hall 2 ", "Hall 3 ", "ETF ", "Hall 4 ", "Hall 5 ", "Wing A ", "Wing B ", "Off campus" };
            var lasuHostels = new[] { "Volks ", "Post service ", "Alaba ", "First gate ", "Igando ", "Obadore ", "Oko filling ", "Estate gate ", "Iyana school ", "Iba junction ", "Okoko ", "Idiorogbo ", "Franklass ", "PPL ", "Off campus" };

            var existingHostels = dbContext.Hostels.ToList();

            foreach (var item in unilagHostels)
            {
                if (existingHostels.Any(e => e.Name.ToLower() == item.ToLower() && e.CampusId == unilag.Id))
                    continue;
                dbContext.Hostels.Add(new Shared.Entities.Hostel { CampusId = unilag.Id, Name = item });
            }

            foreach (var item in yabatechHostels)
            {
                if (existingHostels.Any(e => e.Name.ToLower() == item.ToLower() && e.CampusId == yabatech.Id))
                    continue;
                dbContext.Hostels.Add(new Shared.Entities.Hostel { CampusId = yabatech.Id, Name = item });
            }

            foreach (var item in fceAkokaHostels)
            {
                if (existingHostels.Any(e => e.Name.ToLower() == item.ToLower() && e.CampusId == fceAkoka.Id))
                    continue;
                dbContext.Hostels.Add(new Shared.Entities.Hostel { CampusId = fceAkoka.Id, Name = item });
            }

            foreach (var item in lasuHostels)
            {
                if (existingHostels.Any(e => e.Name.ToLower() == item.ToLower() && e.CampusId == lasu.Id))
                    continue;
                dbContext.Hostels.Add(new Shared.Entities.Hostel { CampusId = lasu.Id, Name = item });
            }

            dbContext.SaveChanges();
        }
    }

    public static class IWebHostExtensions
    {
        public static IWebHost SeedData(this IWebHost webHost, params Action<ApplicationDbContext>[] dbSeedAction)
        {
            var serviceScopeFactory = (IServiceScopeFactory)webHost.Services.GetService(typeof(IServiceScopeFactory));

            using (var scope = serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                dbContext.Database.Migrate();

                foreach (var seedData in dbSeedAction)
                {
                    seedData(dbContext);
                }
            }

            return webHost;
        }

    }
}
