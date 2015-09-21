using CatalogManager.Domain;
using CatalogManager.Domain.Aggregates;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogManager.Persistence.DataContext
{
 
    public class CatalogDbContext : DbContext
    {
        //static CatalogDbContext()
        //{
        //    Database.SetInitializer(new SampleAppInitializer());
        //}

        public CatalogDbContext()
            : base("DefaultConnection")   
        {
            Products = base.Set<Product>();
            Categories = base.Set<Category>();
 
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            modelBuilder.ComplexType<Money>();
            //modelBuilder.ComplexType<Address>();
            //modelBuilder.ComplexType<CreditCard>();

            //modelBuilder.Configurations.Add(new ExpiryDateMap());
            //modelBuilder.Configurations.Add(new FidelityCardMap());
            //modelBuilder.Configurations.Add(new OrderMap());
            //modelBuilder.Configurations.Add(new CurrencyMap());
            //modelBuilder.Configurations.Add(new OrderItemMap());
            //modelBuilder.Configurations.Add(new CustomerMap());
            //modelBuilder.Configurations.Add(new AdminMap());
        }

 

        public DbSet<Category> Categories { get; private set; }
        public DbSet<Product> Products { get; private set; }

    }
}
