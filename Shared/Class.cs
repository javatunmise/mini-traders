//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;
//using Microsoft.Extensions.Hosting;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations.Schema;
//using System.Linq;
//using System.Threading.Tasks;

//namespace site.Repositories.Models
//{
//    public class SiteUser
//    {
//        public int Id { get; set; }
//        public string FirstName { get; set; }
//        public string LastName { get; set; }
//        public string Email { get; set; }
//        public string PhoneNumber { get; set; }
//        public Store Store { get; set; }
//    }

//    public class Store
//    {
//        public int Id { get; set; }
//        public string StoreName { get; set; }
//        public string StoreDescription { get; set; }
//        public string LogoPath { get; set; }
//        public string UploadedDocPath { get; set; }
//        public string ReferrerCode { get; set; }

//        public Campus Campus { get; set; }
//        public Hostel Hostel { get; set; }
//        public int SiteUserId { get; set; }
//        public SiteUser User { get; set; }
//    }

//    public class Campus
//    {
//        public int Id { get; set; }
//        public string Name { get; set; }
//        public ICollection<Hostel> Hostels { get; set; }
//    }

//    public class Hostel
//    {
//        public int Id { get; set; }
//        public string Name { get; set; }
//        public int CampusId { get; set; }
//        public Campus Campus { get; set; }
//    }


//    internal class SiteUserSchemaConfiguration : IEntityTypeConfiguration<SiteUser>
//    {
//        public void Configure(EntityTypeBuilder<SiteUser> builder)
//        {
//            builder.Property(p => p.FirstName).HasMaxLength(50);
//            builder.Property(p => p.LastName).HasMaxLength(50);
//            builder.Property(p => p.PhoneNumber).HasMaxLength(20);
//            builder.Property(p => p.Email).HasMaxLength(255);

//            builder.HasOne(user => user.Store)
//                   .WithOne(store => store.User)
//                   .HasForeignKey<Store>(store => store.SiteUserId);
//        }
//    }

//    internal class StoreSchemaConfiguration : IEntityTypeConfiguration<Store>
//    {
//        public void Configure(EntityTypeBuilder<Store> builder)
//        {
//            builder.Property(p => p.StoreName).HasMaxLength(100);
//            builder.Property(p => p.StoreDescription).HasMaxLength(1024);
//            builder.Property(p => p.LogoPath).HasMaxLength(255);
//            builder.Property(p => p.UploadedDocPath).HasMaxLength(255);
//            builder.Property(p => p.ReferrerCode).HasMaxLength(20);
//        }
//    }

//    internal class HostelSchemaConfiguration : IEntityTypeConfiguration<Hostel>
//    {
//        public void Configure(EntityTypeBuilder<Hostel> builder)
//        {
//            builder.Property(p => p.Name).HasMaxLength(100);
//        }
//    }

//    internal class CampusSchemaConfiguration : IEntityTypeConfiguration<Campus>
//    {
//        public void Configure(EntityTypeBuilder<Campus> builder)
//        {
//            builder.Property(p => p.Name).HasMaxLength(100);
//        }
//    }
//}
