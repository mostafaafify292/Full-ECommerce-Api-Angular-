
using Ecom.Core.Entites.Product;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {  
        }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Photo> Photos { get; set; }
        public virtual DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(System.Reflection.Assembly.GetExecutingAssembly());
        }

    }
}
