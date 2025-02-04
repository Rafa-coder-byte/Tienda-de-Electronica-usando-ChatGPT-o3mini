using mi_tienda_de_electrónica.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;


namespace mi_tienda_de_electrónica.Data
{ 
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Review> Reviews { get; set; } // Nueva tabla para reseñas

        // Opcional: Configurar relaciones si es necesario
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.Product)
                .WithMany(p => p.Reviews)
                .HasForeignKey(r => r.ProductId);
        }
    }
 }
