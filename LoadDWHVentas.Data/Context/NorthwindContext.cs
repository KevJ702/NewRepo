﻿
using LoadDWHVentas.Data.Context.Entities.Northwind;
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
        #endregion
    }
}