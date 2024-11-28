
using Microsoft.EntityFrameworkCore;

namespace LoadDWHVentas.Models.Models;

public partial class NorthWindContext : DbContext
{
    public NorthWindContext(DbContextOptions<NorthWindContext> options)
        : base(options)
    {
    }

    public virtual DbSet<VwServedCustomer> VwServedCustomers { get; set; }

    public virtual DbSet<VwVenta> VwVentas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<VwServedCustomer>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("VwServedCustomers");

            entity.Property(e => e.EmployeeName)
                .IsRequired()
                .HasMaxLength(31);
        });

        modelBuilder.Entity<VwVenta>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("VwVentas");

            entity.Property(e => e.CompanyName)
                .IsRequired()
                .HasMaxLength(40);
            entity.Property(e => e.Country).HasMaxLength(15);
            entity.Property(e => e.CustomerID)
                .IsRequired()
                .HasMaxLength(5)
                .IsFixedLength();
            entity.Property(e => e.CustomerName)
                .IsRequired()
                .HasMaxLength(40);
            entity.Property(e => e.EmployeeName)
                .IsRequired()
                .HasMaxLength(31);
            entity.Property(e => e.ProductName)
                .IsRequired()
                .HasMaxLength(40);
            entity.Property(e => e.TotalSales).HasColumnType("money");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}