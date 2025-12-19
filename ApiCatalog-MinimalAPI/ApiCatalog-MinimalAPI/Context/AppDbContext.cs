
using ApiCatalog_MinimalAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalog_MinimalAPI.Context
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Product>? Products { get; set; }
        public DbSet<Category>? Categories { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasKey(C => C.CategoryId);

            modelBuilder.Entity<Category>()
                .Property(c=> c.Name)
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<Category>()
                .Property(c => c.Description)
                .HasMaxLength(150)
                .IsRequired();

            modelBuilder.Entity<Product>().HasKey(p=> p.ProductId);

            modelBuilder.Entity<Product>()
                .Property(p=> p.Name)
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<Product>()
                .Property(p => p.Description)
                .HasMaxLength(150);

            modelBuilder.Entity<Product>()
                .Property(p => p.ImageUrl)
                .HasMaxLength(100);

            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasPrecision(14, 2);

            //Relacionamento
             modelBuilder.Entity<Product>()
                .HasOne(c=> c.Category)
                .WithMany(p=> p.Products)
                .HasForeignKey(p=>p.CategoryId);
        }
    }
}
