

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoadDWHVentas.Data.Entities.DWHVentas
{
    [Table("DimShippers")]
    public class DimShippers
    {
        [Key]
        public int ShipperKey { get; set; }

        public int ShipperID  { get; set; }

        public string CompanyName { get; set; }
    }
}
