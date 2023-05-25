using Microsoft.EntityFrameworkCore;

namespace DemoToken.Models
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Product>().ToTable("products").HasKey(p => p.Id);
            modelBuilder.Entity<UserModel>().ToTable("users").HasKey(u => u.Id);
            modelBuilder.Entity<UserModel>().HasIndex(u => u.Email).IsUnique();
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<UserModel> Users { get; set; }
    }
}