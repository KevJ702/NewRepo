

namespace LoadDWHVentas.Data.Entities.Northwind
{
    public class VwVenta
    {
        public string CustomerId { get; set; }

        public string CustomerName { get; set; }

        public int EmployeeId { get; set; }

        public string EmployeeName { get; set; }

        public int ShipperId { get; set; }

        public string CompanyName { get; set; }

        public int? DateKey { get; set; }

        public int? Year { get; set; }

        public int? Month { get; set; }

        public decimal? TotalSales { get; set; }

        public int? Quantity { get; set; }
    }
}
