using LoadDWHVentas.Data.Context;
using LoadDWHVentas.Data.Interfaces;
using LoadDWHVentas.Data.Services;
using LoadDWHVentas.WorkerService;
using Microsoft.EntityFrameworkCore;

internal class Program
{
    private static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }
    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
        .ConfigureServices((hostContext, services) => {

            services.AddDbContextPool<NorthwindContext>(options => options.UseSqlServer(hostContext.Configuration.GetConnectionString("DbNorthwind")));

            services.AddDbContextPool<DbSalesContext>(options => options.UseSqlServer(hostContext.Configuration.GetConnectionString("DbSales")));

            services.AddScoped<IDataServiceDWHVentas, DataServiceDWHVentas>();

            services.AddHostedService<Worker>();
        });
}