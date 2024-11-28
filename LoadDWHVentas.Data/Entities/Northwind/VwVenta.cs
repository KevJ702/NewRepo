#nullable disable

namespace LoadDWHVentas.Data.Entities.Northwind
{
    public  class VwVenta
    {
        public string? CustomerID { get; set; }

        public string? CustomerName { get; set; }

        public int employeeId { get; set; }

        public string? EmployeeName { get; set; }

        public int ShipperID { get; set; }

        public string? CompanyName { get; set; }

        public string? Country { get; set; }

        public int ProductId { get; set; }

        public string? ProductName { get; set; }

        public int? DateKey { get; set; }

        public int? Year { get; set; }

        public int? Month { get; set; }

        public decimal? TotalSales { get; set; }

        public int? Quantity { get; set; }
    }
}
