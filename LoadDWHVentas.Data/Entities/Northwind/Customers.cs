

using System.ComponentModel.DataAnnotations;

namespace LoadDWHVentas.Data.Entities.Northwind
{
    public class Customers
    {
        [Key]
        public string CustomerId { get; set; }

        public string CompanyName { get; set; }
    }
}
