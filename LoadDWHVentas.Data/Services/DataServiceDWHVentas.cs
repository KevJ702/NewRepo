

using LoadDWHVentas.Data.Context;
using LoadDWHVentas.Data.Entities.DWHVentas;
using LoadDWHVentas.Data.Entities.DwVentas;
using LoadDWHVentas.Data.Entities.Northwind;
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


                // Borra los datos  de la tabla Employees si existen


                int[] employeeIds = Employees.Select(emp => emp.EmployeeId).ToArray();

                await _salesContext.DimEmployees.Where(cd => employeeIds.Contains(cd.EmployeeId))
                                                            .AsNoTracking()
                                                            .ExecuteDeleteAsync();


                // Carga la dimension de empleados
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

                

                // Borra los datos  de la tabla Products si existen

                int[] productsIds = products.Select(c => c.ProductID).ToArray();

                await _salesContext.DimProducts.Where(c => productsIds.Contains(c.ProductID))
                                                .AsNoTracking()
                                                .ExecuteDeleteAsync();

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

        private async Task<OperationResult> LoadDimShippers()
        {
            OperationResult result = new OperationResult();

            try
            {
                // Obtener los transportistas de la base de datos Northwind
                var Shippers = _northwindContext.Shippers.AsNoTracking().Select(Ship => new DimShippers()
                {
                    ShipperID = Ship.ShipperId,
                    CompanyName = Ship.CompanyName
                }).ToList();

                // Borra los datos  de la tabla Shippers si existen

                int[] shipperIds = Shippers.Select(ship => ship.ShipperID).ToArray();

                await _salesContext.DimShippers.Where(ship => shipperIds.Contains(ship.ShipperID))
                                                            .AsNoTracking()
                                                            .ExecuteDeleteAsync();
                // Carga la dimension de transportistas               
                await _salesContext.DimShippers.AddRangeAsync(Shippers);

                await _salesContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"Error cargando la dimension de transportistas: {ex.Message}";
            }
            return result;
        }

        private async Task<OperationResult> LoadDimCustomers()
        {
            OperationResult result = new OperationResult();

            try
            {
                // Obtener los clientes de la base de datos Northwind
                var Customers = _northwindContext.Customers.AsNoTracking().Select(Cust => new DimCustomers()
                {
                    CustomerName = Cust.CompanyName,
                    CustomerId = Cust.CustomerId
                }).ToList();

                // Borra los datos  de la tabla Customers si existen
                
                string[] customerIds = Customers.Select(cust => cust.CustomerId).ToArray();

                await _salesContext.DimCustomers.Where(cust => customerIds.Contains(cust.CustomerId))
                                                .AsNoTracking()
                                                .ExecuteDeleteAsync();

                // Carga la dimension de clientes
                await _salesContext.DimCustomers.AddRangeAsync(Customers);

                await _salesContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"Error cargando la dimension de clientes: {ex.Message}";
            }
            return result;


        }

        private async Task<OperationResult> LoadFactSales()
        {
            OperationResult result = new OperationResult();

            try
            {
                var ventas = await _northwindContext.VwVentas.AsNoTracking().ToListAsync(); 
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"Error cargando el fact de ventas: {ex.Message}";
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
                await LoadDimCustomers();
                await LoadDimShippers();

                await LoadFactSales();
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
