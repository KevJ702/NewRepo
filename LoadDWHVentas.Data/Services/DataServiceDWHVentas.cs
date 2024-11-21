

using LoadDWHVentas.Data.Context;
using LoadDWHVentas.Data.Entities.DwVentas;
using LoadDWHVentas.Data.Interfaces;
using LoadDWHVentas.Data.Result;
using Microsoft.EntityFrameworkCore;

namespace LoadDWHVentas.Data.Services
{
    public class DataServiceDWHVentas : IDataServiceDWHVentas
    {
        private readonly NorthwindContext _northwindContext;
        private readonly DbSalesContext _salesContext;

        public DataServiceDWHVentas(NorthwindContext northwindContext,
                                    DbSalesContext salesContext)
        {
            _northwindContext = northwindContext;
            _salesContext = salesContext;
        }
        private async Task<OperationResult> LoadDimEmployees()
        {
            OperationResult  result = new OperationResult();

            try
            {
                // Obtener los empleados de la base de datos Northwind
                var Employees = _northwindContext.Employees.AsNoTracking().Select(emp => new DimEmployees() 
                {
                    EmployeeId = emp.EmployeeId,
                    EmployeeName = string.Concat(emp.FirstName," ",emp.LastName)
                }).ToList();

                // Carga la dimension de empleados
                //If code doesn't work, change The Employees below to a lowercase
                await _salesContext.DimEmployees.AddRangeAsync(Employees);

                await _salesContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"Error cargando la dimension de empleado: {ex.Message}"; 
            }
            return result;
        }

        private async Task<OperationResult> LoadDimProducts()
        {
            OperationResult result = new OperationResult();
            try
            {
                // Obtener los productos y las categorías de Northwind
                var products = (from product in _northwindContext.Products
                             join category in _northwindContext.Categories on product.CategoryId equals category.CategoryId
                             select new DimProducts()
                             {
                                 CategoryID = category.CategoryId,
                                 ProductName = product.ProductName,
                                 CategoryName = category.CategoryName,
                                 ProductID = product.ProductId
                             }).AsNoTracking().ToList();

                // Carga la dimension de productos categorias
                await _salesContext.DimProducts.AddRangeAsync(products);
                await _salesContext.SaveChangesAsync();
            }

            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"Error cargando la dimension de producto y categoría: {ex.Message}";
            }
            return result;
        }

        public async Task<OperationResult> LoadDWH()
        {
            OperationResult result = new OperationResult();

           try
            {
                await LoadDimEmployees();
                await LoadDimProducts();
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"Error cargando el DWH : {ex.Message}";
            }

            return result;
        }
    }
}
