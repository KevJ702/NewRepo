using LoadDWHVentas.Data.Entities.DWHVentas;
using LoadDWHVentas.Data.Entities.DwVentas;
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

        #endregion
    }
}
