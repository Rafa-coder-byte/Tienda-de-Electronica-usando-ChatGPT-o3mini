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
    }
 }
