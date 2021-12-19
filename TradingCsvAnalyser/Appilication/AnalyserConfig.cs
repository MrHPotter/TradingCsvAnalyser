using System;
using Microsoft.Extensions.Configuration;
using TradingCsvAnalyser.Models.Database;

namespace TradingCsvAnalyser.Appilication;

public class AnalyserConfig : IAnalyserConfig
{
    private IConfiguration _configuration;

    public AnalyserConfig(IConfiguration configuration)
    {
        _configuration = configuration;
    }


    public string ConnectionString => _configuration["Appsettings:ConnectionString"];

    public DbProvider DbProvider => Enum.Parse<DbProvider>(_configuration["Appsettings:DbType"], true);

    public bool UseLazyLoading => Boolean.Parse(_configuration["Appsettings:UseEfCoreLazyLoading"]);
    
    public bool LogEfCore => Boolean.Parse(_configuration["Appsettings:UseEfCoreLogging"]);
}