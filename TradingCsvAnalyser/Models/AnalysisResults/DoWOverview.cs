using TradingCsvAnalyser.Models.Enums;
using TradingCsvAnalyser.Models.HelperModels;

namespace TradingCsvAnalyser.Models.AnalysisResults;

public class DoWOverview : DayOfWeekData
{
    public DoWOverview(string symbol, string range ,string method)
    {
        Symbol = symbol;
        Method = method;
        Range = range;
    }

    public DoWOverview(DayOfWeekData data, string symbol,string range, string method, DayFilter dayFilter, DateRange dateRange) : base(data)
    {
        Symbol = symbol;
        Method = method;
        DayFilter = dayFilter;
        Range = range;
        DateRange = dateRange;
    }
    public string Symbol { get; set; }
    
    public string Method { get; set; }
    
    public string Range { get; set; }
    
    public DayFilter DayFilter { get; set; }
    
    public DateRange DateRange { get; set; }
}