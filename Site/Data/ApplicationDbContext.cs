using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Shared.Entities;
using site.Data.Configurations;

namespace Site.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<SiteUser> SiteUsers { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<Campus> Campuses { get; set; }
        public DbSet<Hostel> Hostels { get; set; }

        public DbSet<Referral> Referrals { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            var schemaConfigs = new dynamic []
            {
                new SiteUserSchemaConfiguration(),
                new StoreSchemaConfiguration(),
                new CampusSchemaConfiguration(),
                new HostelSchemaConfiguration()
            };

            foreach (var schema in schemaConfigs)
            {
                builder.ApplyConfiguration(schema);
            }

            base.OnModelCreating(builder);
        }
    }
}
