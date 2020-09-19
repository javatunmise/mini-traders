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

        public DbSet<Product> Products { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<ProductTag> ProductTags { get; set; }
        public DbSet<Referral> Referrals { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductReview> ProductReviews { get; set; }

        public DbSet<Chat> Chats { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }

        public DbSet<Shared.Entities.Site> Sites { get; set; }
        public DbSet<SiteImage> SiteImages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            var schemaConfigs = new dynamic []
            {
                new CampusSchemaConfiguration(),
                new HostelSchemaConfiguration(),
                new SiteUserSchemaConfiguration(),
                new StoreSchemaConfiguration(),
                new ReferralSchemaConfiguration(),
                new CategorySchemaConfiguration(),
                new ProductSchemaConfiguration(),
                new ProductReviewSchemaConfiguration(),
                new ChatSchemaConfiguration(),
                new ChatMessageSchemaConfiguration()
            };

            foreach (var schema in schemaConfigs)
            {
                builder.ApplyConfiguration(schema);
            }

            base.OnModelCreating(builder);

            builder.Entity<ProductTag>().HasKey(e => new { e.ProductId, e.TagId });

        }
    }
}
