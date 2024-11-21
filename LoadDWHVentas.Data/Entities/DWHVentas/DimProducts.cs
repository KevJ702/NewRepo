using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace LoadDWHVentas.Data.Entities.DwVentas
{
    [Table("DimProducts")]
    public class DimProducts
    {
        [Key]
        public int ProductKey { get; set; }

        public int ProductID { get; set; }

        public string? ProductName { get; set; }

        public int CategoryID { get; set; }

        public string? CategoryName { get; set; }
    }
}
