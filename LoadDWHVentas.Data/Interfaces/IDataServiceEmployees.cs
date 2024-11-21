

using LoadDWHVentas.Data.Entities.DwVentas;
using LoadDWHVentas.Data.Result;

namespace LoadDWHVentas.Data.Interfaces
{
    public interface IDataServiceDWHVentas
    {
        

        Task<OperationResult> LoadDWH();


    }
}
