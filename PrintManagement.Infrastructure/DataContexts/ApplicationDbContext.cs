using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PrintManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintManagement.Infrastructure.DataContexts
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public ApplicationDbContext() { }
        public virtual DbSet<ConfirmEmail> ConfirmEmails { get; set; }
        public virtual DbSet<RefreshToken> RefreshTokens { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Permissions> Permissions { get; set; }
        public virtual DbSet<Bill> Bills { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<CustomerFeedback> CustomerFeedbacks { get; set; }
        public virtual DbSet<Delivery> Deliveries { get; set; }
        public virtual DbSet<Design> Designs { get; set; }
        public virtual DbSet<ImportCoupon> ImportCoupons { get; set; }
        public virtual DbSet<KeyPerformanceIndicators> KeyPerformanceIndicators { get; set; }
        public virtual DbSet<PrintJob> PrintJobs { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<Resource> Resources { get; set; }
        public virtual DbSet<ResourceProperty> ResourceProperties { get; set; }
        public virtual DbSet<ResourcePropertyDetail> ResourcePropertyDetails { get; set; }
        public virtual DbSet<ResourceForPrintJob> ResourceForPrintJobs { get; set; }
        public virtual DbSet<ShippingMethod> ShippingMethods { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<ConfirmReceiptOfGoodsFromCustomer> ConfirmReceiptOfGoodsFromCustomers { get; set; }
        public virtual DbSet<ResourceType> ResourceTypes { get; set; }
        public virtual DbSet<Team> Teams { get; set; }
        //protected override void OnModelCreating(ModelBuilder builder)
        //{
        //    base.OnModelCreating(builder);
        //    SeedTeams(builder);
        //    SeedRoles(builder);

        //}

        //private static void SeedRoles(ModelBuilder builder)
        //{
        //    builder.Entity<Role>().HasData
        //        (
        //            new Role() { Id = Guid.NewGuid(), RoleCode = "Admin", RoleName = "Admin" },
        //            new Role() { Id = Guid.NewGuid(), RoleCode = "Manager", RoleName = "Manager" },
        //            new Role() { Id = Guid.NewGuid(), RoleCode = "Leader", RoleName = "Leader" },
        //            new Role() { Id = Guid.NewGuid(), RoleCode = "Designer", RoleName = "Designer" },
        //            new Role() { Id = Guid.NewGuid(), RoleCode = "Deliver", RoleName = "Deliver" },
        //            new Role() { Id = Guid.NewGuid(), RoleCode = "Employee", RoleName = "Employee" }
        //        );
        //}

        //private static void SeedTeams(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Team>().HasData
        //        (
        //            new Team() { Id = Guid.NewGuid(), IsActive = true, CreateTime = DateTime.Now, Description = "Phòng ban kinh doanh", Name = "Sales" },
        //            new Team() { Id = Guid.NewGuid(), IsActive = true, CreateTime = DateTime.Now, Description = "Phòng ban kỹ thuật", Name = "Technical" },
        //            new Team() { Id = Guid.NewGuid(), IsActive = true, CreateTime = DateTime.Now, Description = "Phòng ban giao hàng", Name = "Delivery" }
        //        );
        //}

        public async Task<int> CommitChangesAsync()
        {
            return await base.SaveChangesAsync();
        }

        public DbSet<TEntity> SetEntity<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }
    }
}
