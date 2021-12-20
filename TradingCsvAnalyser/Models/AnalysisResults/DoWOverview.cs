using TradingCsvAnalyser.Models.Enums;

namespace TradingCsvAnalyser.Models.AnalysisResults;

public class DoWOverview : DayOfWeekData
{
    public DoWOverview(string symbol, string range ,string method)
    {
        Symbol = symbol;
        Method = method;
        Range = range;
    }

    public DoWOverview(DayOfWeekData data, string symbol,string range, string method, DayFilter dayFilter) : base(data)
    {
        Symbol = symbol;
        Method = method;
        DayFilter = dayFilter;
        Range = range;
    }
    public string Symbol { get; set; }
    
    public string Method { get; set; }
    
    public string Range { get; set; }
    
    public DayFilter DayFilter { get; set; }
}