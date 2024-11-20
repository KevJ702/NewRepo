using System.ComponentModel.DataAnnotations.Schema;

namespace LoadDWHVentas.Data.Entities.DwVentas
{
    [Table("DimEmployees")]
    public class DimEmployees
    {
        public int EmployeeId { get; set; }

        public int EmployeeKey { get; set; }

        public string? EmployeeName { get; set; }
    }
}
