using System;
using CsvHelper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TradingCsvAnalyser.Appilication;
using TradingCsvAnalyser.DataProviders;
using TradingCsvAnalyser.Managers;
using TradingCsvAnalyser.Models.Database;
using TradingCsvAnalyser.Windows;

namespace TradingCsvAnalyser.Extensions;

public static class ServiceCollectionExtensions
{
    
    /// <summary>
    /// a basic console logger factory
    /// </summary>
    public static ILoggerFactory ConsoleLoggerFactory { get; set; } = new LoggerFactory();
    
    public static void ConfigureServices(this IServiceCollection services)
    {
        services.AddSingleton<IUnitOfWork, UnitOfWork>();
        services.AddTransient<IReader, CsvReader>();
        services.AddTransient<IDayOfWeekDataManager, DayOfWeekDataManager>();
        services.AddTransient(typeof(ImportWindow));
        services.AddTransient(typeof(OverView));
    }
    
}