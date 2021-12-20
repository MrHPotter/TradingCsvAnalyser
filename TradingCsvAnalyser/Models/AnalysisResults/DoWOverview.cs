namespace TradingCsvAnalyser.Models.AnalysisResults;

public class DoWOverview : DayOfWeekData
{
    public DoWOverview(string symbol, string method)
    {
        Symbol = symbol;
        Method = method;
    }

    public DoWOverview(DayOfWeekData data, string symbol, string method) : base(data)
    {
        Symbol = symbol;
        Method = method;
    }
    public string Symbol { get; set; }
    
    public string Method { get; set; }
}