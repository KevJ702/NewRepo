

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace LoadDWHVentas.Data.Models;

[Table("FactCustomersServed")]
public  class FactCustomersServed
{
    [Key]
    public int ClienteAtendidoId { get; set; }

    public int EmployeeKey { get; set; }

    public int? TotalClientes { get; set; }
}