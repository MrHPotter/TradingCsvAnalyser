﻿using System;
using CsvHelper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TradingCsvAnalyser.Appilication;
using TradingCsvAnalyser.DataProviders;
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
        services.AddTransient<IUnitOfWork, UnitOfWork>();
        services.AddTransient<IReader, CsvReader>();
        services.AddTransient(typeof(ImportWindow));
    }

    /// <summary>
    /// Proprietary Method for Adding the DBContext Factory with custom Settings via config
    /// </summary>
    /// <param name="services"></param>
    /// <param name="config"></param>
    /// <typeparam name="TContext"></typeparam>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static void AddDbContextFactory<TContext>(this IServiceCollection services, IAnalyserConfig config)
        where TContext : DbContext
    {
        Action<DbContextOptionsBuilder> useDb = config.DbProvider switch
        {
            DbProvider.SqLite => builder => builder.UseSqlite(),
            DbProvider.PostgreSql => builder => builder.UseNpgsql(config.ConnectionString),
            _ => throw new ArgumentOutOfRangeException(nameof(config.DbProvider),
                $"This {nameof(DbProvider)} is not supported")
        };

        services.AddDbContextFactory<TContext>(options =>
        {
            useDb(options);
            if (config.UseLazyLoading) options = options.UseLazyLoadingProxies();
            if (config.LogEfCore) options.UseLoggerFactory(ConsoleLoggerFactory);
        });
    }
}