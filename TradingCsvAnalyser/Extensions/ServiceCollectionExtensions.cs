using CsvHelper;
using Microsoft.Extensions.DependencyInjection;

namespace TradingCsvAnalyser.Extensions;

public static class ServiceCollectionExtensions
{
    public static void ConfigureServices(this IServiceCollection services)
    {
        services.AddTransient(typeof(MainWindow));
        services.AddTransient<IReader, CsvReader>();
    }
}