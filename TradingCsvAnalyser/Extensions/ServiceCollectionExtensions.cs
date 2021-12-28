using CsvHelper;
using Microsoft.Extensions.DependencyInjection;
using TradingCsvAnalyser.DataProviders;
using TradingCsvAnalyser.Factories;
using TradingCsvAnalyser.Managers;
using TradingCsvAnalyser.Windows;

namespace TradingCsvAnalyser.Extensions;

public static class ServiceCollectionExtensions
{

    public static void ConfigureServices(this IServiceCollection services)
    {
        services.AddSingleton<IUnitOfWork, UnitOfWork>();
        services.AddTransient<IReader, CsvReader>();
        services.AddTransient<IDayOfWeekDataManager, DayOfWeekDataManager>();
        services.AddSingleton<IWindowFactory, WindowFactory>();
        services.AddTransient(typeof(ImportWindow));
        services.AddTransient(typeof(OverView));
        services.AddTransient(typeof(DetailAnalysisWindow));
        services.AddTransient(typeof(FollowDayWindow));
    }
    
}