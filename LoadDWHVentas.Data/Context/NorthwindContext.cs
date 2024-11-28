using LoadDWHVentas.Data.Entities.Northwind;

using Microsoft.EntityFrameworkCore;

namespace LoadDWHVentas.Data.Context
{
    public  class NorthwindContext : DbContext
    {
        public  NorthwindContext(DbContextOptions<NorthwindContext> options) : base(options) 
        {

        }
        #region"Db Sets"
        public DbSet<Category> Categories { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<Shippers> Shippers { get; set; }

        public DbSet<Customers> Customers { get; set; }

        public DbSet<VwServedCustomer> VwServedCustomers { get; set; }

        public DbSet<VwVenta> VwVentas { get; set; }

        #endregion



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<VwServedCustomer>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToView("VwServedCustomers");

                //        entity.Property(e => e.EmployeeName)
                //            .IsRequired()
                //            .HasMaxLength(31);
            });

            modelBuilder.Entity<VwVenta>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToView("VwVentas");

                //        entity.Property(e => e.CompanyName)
                //            .IsRequired()
                //            .HasMaxLength(40);
                //        entity.Property(e => e.Country).HasMaxLength(15);
                //        entity.Property(e => e.CustomerID)
                //            .IsRequired()
                //            .HasMaxLength(5)
                //            .IsFixedLength();
                //        entity.Property(e => e.CustomerName)
                //            .IsRequired()
                //            .HasMaxLength(40);
                //        entity.Property(e => e.EmployeeName)
                //            .IsRequired()
                //            .HasMaxLength(31);
                //        entity.Property(e => e.ProductName)
                //            .IsRequired()
                //            .HasMaxLength(40);
                //        entity.Property(e => e.TotalSales).HasColumnType("money");
            });

      
        }

    }
}
