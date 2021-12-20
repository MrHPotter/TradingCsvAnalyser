using TradingCsvAnalyser.Models.Enums;

namespace TradingCsvAnalyser.Models.HelperModels;

public class DoWDefaultParameters
{
    public DoWDefaultParameters(CandleRange rangeType, string symbol, DayFilter dayFilter)
    {
        RangeType = rangeType;
        Symbol = symbol;
        DayFilter = dayFilter;
    }

    public CandleRange RangeType { get; private set; }
    public string Symbol { get; private set; }
    public DayFilter DayFilter { get; private set; }
}