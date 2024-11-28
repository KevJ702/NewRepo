

using LoadDWHVentas.Data.Context;
using LoadDWHVentas.Data.Entities.DWHVentas;
using LoadDWHVentas.Data.Entities.DwVentas;
using LoadDWHVentas.Data.Interfaces;
using LoadDWHVentas.Data.Models;
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

        

        private async Task<OperationResult> LoadFactOrder()
        {
            OperationResult result = new OperationResult() {Success = true };

            try
            {
                var ventas = await _northwindContext.VwVentas.AsNoTracking().ToListAsync();

                int[] ordersId = await _salesContext.FactOrders.Select(cd => cd.OrderNumber).ToArrayAsync();

                if (ordersId.Any())
                {
                    await _salesContext.FactOrders.Where(cd => ordersId.Contains(cd.OrderNumber)).AsNoTracking().ExecuteDeleteAsync();
                }

                foreach (var venta in ventas)
                {
                    var customer = await _salesContext.DimCustomers.SingleOrDefaultAsync(cust => cust.CustomerId == venta.CustomerID);
                    var employee = await _salesContext.DimEmployees.SingleOrDefaultAsync(emp => emp.EmployeeId == venta.employeeId);
                    var shipper = await _salesContext.DimShippers.SingleOrDefaultAsync(ship => ship.ShipperID == venta.ShipperID);
                    var product = await _salesContext.DimProducts.SingleOrDefaultAsync(pro => pro.ProductID == venta.ProductId);


                    FactOrder factOrder = new FactOrder()
                    {
                        Quantity = venta.Quantity.Value,
                        Country = venta.Country,
                        CustomerKey = customer.CustomerKey,
                        EmployeeKey = employee.EmployeeKey,
                        DateKey = venta.DateKey.Value,
                        ProductKey = product.ProductKey,
                        ShipperKey = shipper.ShipperKey,
                        TotalSales = Convert.ToDecimal(venta.TotalSales)

                    };

                    await _salesContext.FactOrders.AddAsync(factOrder);

                    await _salesContext.SaveChangesAsync();
                }                

                result.Success = true;
            }
        
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"Error cargando el fact de ventas: {ex.Message}";
            }

            return result;
        }
        private async Task<OperationResult> LoadFactCustomerServed()
        {
            OperationResult result = new OperationResult();

            try
            {
                var customerServed = await _northwindContext.VwServedCustomers.AsNoTracking().ToListAsync();

                int[] customerIds = _salesContext.FactCustomerServed.Select(cli => cli.ClienteAtendidoId).ToArray();

                if (customerIds.Any())
                {
                    await _salesContext.FactCustomerServed.Where(fact => customerIds.Contains(fact.ClienteAtendidoId)).AsNoTracking().ExecuteDeleteAsync();
                }

                foreach (var customer in customerServed)
                {                   
                    var employee = await _salesContext.DimEmployees.SingleOrDefaultAsync(emp => emp.EmployeeId == customer.EmployeeId);

                    await _salesContext.FactCustomerServed.AddAsync(new FactCustomersServed()
                    {
                        EmployeeKey = employee.EmployeeKey,
                        TotalClientes = customer.TotalCustomersServed
                    });                    

   

                    await _salesContext.SaveChangesAsync();
                }

                result.Success = true;
            }

            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"Error cargando el fact de clientes atendidos: {ex.Message}";
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

                await LoadFactOrder();
                await LoadFactCustomerServed();
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
