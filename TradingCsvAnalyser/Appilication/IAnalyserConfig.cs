using System.Data;
using TradingCsvAnalyser.Models.Database;

namespace TradingCsvAnalyser.Appilication;

public interface IAnalyserConfig
{
    public string ConnectionString { get; }
    
    public DbProvider DbProvider { get; }
    
    public bool UseLazyLoading { get; }
    
    public bool LogEfCore { get; }
}