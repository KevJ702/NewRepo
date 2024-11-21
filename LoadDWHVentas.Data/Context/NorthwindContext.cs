using LoadDWHVentas.Data.Entities.Northwind;
using Microsoft.EntityFrameworkCore;

namespace LoadDWHVentas.Data.Context
{
    public partial class NorthwindContext : DbContext
    {
        public NorthwindContext(DbContextOptions<NorthwindContext> options) : base(options) 
        {

        }
        #region"Db Sets"
        public DbSet<Category> Categories { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<Shippers> Shippers { get; set; }

        public DbSet<Customers> Customers { get; set; }

        public DbSet<VwVenta> VwVentas { get; set; }
        #endregion
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<VwVenta>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToView("VwVentas");

                entity.Property(e => e.CompanyName)
                    .IsRequired()
                    .HasMaxLength(40);
                entity.Property(e => e.CustomerId)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsFixedLength()
                    .HasColumnName("CustomerID");
                entity.Property(e => e.CustomerName)
                    .IsRequired()
                    .HasMaxLength(40);
                entity.Property(e => e.EmployeeId).HasColumnName("employeeId");
                entity.Property(e => e.EmployeeName)
                    .IsRequired()
                    .HasMaxLength(31);
                entity.Property(e => e.ShipperId).HasColumnName("ShipperID");
                entity.Property(e => e.TotalSales).HasColumnType("money");
            });
        }
    }
}
