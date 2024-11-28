
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoadDWHVentas.Data.Models;
[Table("FactOrders")]
public class FactOrder
{
    [Key]
    public int OrderNumber { get; set; }   

    public int ShipperKey { get; set; }

    public string? Country { get; set; }

    public int ProductKey { get; set; }

    public int EmployeeKey { get; set; }

    public int CustomerKey { get; set; }

    public int DateKey { get; set; }

    public decimal TotalSales { get; set; }

    public int Quantity { get; set; }
}