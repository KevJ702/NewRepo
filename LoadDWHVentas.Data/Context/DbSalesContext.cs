#nullable disable
using LoadDWHVentas.Data.Entities.DWHVentas;
using LoadDWHVentas.Data.Entities.DwVentas;
using LoadDWHVentas.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace LoadDWHVentas.Data.Context
{
    public class DbSalesContext : DbContext
    {
        public DbSalesContext(DbContextOptions<DbSalesContext> options) : base(options)
        {

        }
        #region"Db Sets"
        public DbSet<DimEmployees> DimEmployees { get; set; }
        public DbSet<DimProducts> DimProducts { get; set; }
        public DbSet<DimShippers> DimShippers { get; set; }
        public DbSet<DimCustomers> DimCustomers { get; set; }

        public DbSet<FactOrder> FactOrders { get; set; }
        public DbSet<FactCustomersServed> FactCustomerServed { get; set; }



        #endregion
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FactOrder>(entity =>
            {
                entity.HasKey(e => e.OrderNumber);

                entity.HasIndex(e => e.CustomerKey, "FactOrders CustomerKey");

                entity.HasIndex(e => e.DateKey, "FactOrders DateKey");

                entity.HasIndex(e => e.EmployeeKey, "FactOrders EmployeeKey");

                entity.HasIndex(e => e.ShipperKey, "FactOrders ShipperKey");

                entity.Property(e => e.Country)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.TotalSales).HasColumnType("decimal(18, 2)");
            });

            //OnModelCreating(modelBuilder);
        }
    }
}
