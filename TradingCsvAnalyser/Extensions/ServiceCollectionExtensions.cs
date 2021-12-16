using CsvHelper;
using Microsoft.Extensions.DependencyInjection;
using TradingCsvAnalyser.Managers;

namespace TradingCsvAnalyser.Extensions;

public static class ServiceCollectionExtensions
{
    public static void ConfigureServices(this IServiceCollection services)
    {
        services.AddTransient(typeof(MainWindow));
        services.AddTransient<ICsvImporter, CsvManager>();
        services.AddTransient<IReader, CsvReader>();
    }
}