
using System.ComponentModel.DataAnnotations;

namespace LoadDWHVentas.Data.Entities.Northwind
{
    public class Shippers
    {
        [Key]
        public int ShipperId { get; set; }

        public string CompanyName { get; set; }
    }
}
