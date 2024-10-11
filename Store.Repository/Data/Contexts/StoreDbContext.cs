using Microsoft.EntityFrameworkCore;
using Store.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repository.Data.Contexts
{ 
    public class StoreDbContext : DbContext {


        //To Allow Clr to Inject the DbContext that i've sent his options in the Main , from the app settings.json
        public StoreDbContext(DbContextOptions<StoreDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            
            base.OnModelCreating(modelBuilder);


        }


        public DbSet<Product> Products { get; set; }
        public DbSet<ProductBrand> Brands { get; set; }
        public DbSet<ProductType> Types { get; set; }

    }

}

