using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CivicTrack.Web.Domain.Entities;

namespace CivicTrack.Web.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext(options)
    {
        public DbSet<ServiceRequest> ServiceRequests { get; set; }
        public DbSet<ServiceCategory> ServiceCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ServiceRequest>()
                .HasKey(request => request.Id);

            builder.Entity<ServiceCategory>()
                .HasKey(category => category.Id);

            builder.Entity<ServiceRequest>()
                .HasOne(request => request.ServiceCategory)
                .WithMany(category => category.ServiceRequests)
                .HasForeignKey(request => request.ServiceCategoryId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ServiceCategory>()
                .HasIndex(category => category.Name)
                .IsUnique();

            builder.Entity<ServiceCategory>()
                .Property(category => category.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Entity<ServiceRequest>()
                .Property(request => request.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Entity<ServiceRequest>()
                .Property(request => request.Description)
                .IsRequired()
                .HasMaxLength(2000);
        }

    }
}
