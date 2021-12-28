using System;
using TradingCsvAnalyser.Models.Enums;

namespace TradingCsvAnalyser.Models.HelperModels;

public class DoWDefaultParameters
{
    public DoWDefaultParameters(CandleRange rangeType, string symbol, DayFilter dayFilter
        , DateRange dateRange)
    {
        RangeType = rangeType;
        Symbol = symbol;
        DayFilter = dayFilter;
        DateRange = dateRange;
    }

    public CandleRange RangeType { get; set; }
    public string Symbol { get; set; }
    public DayFilter DayFilter { get; set; }
    
    public DateRange DateRange { get; set; }
}