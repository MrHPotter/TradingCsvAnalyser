using System;
using TradingCsvAnalyser.Models.Enums;

namespace TradingCsvAnalyser.Models.HelperModels.FollowDayAnalysis;

public class FollowDayParameters
{
    public FollowDayParameters(DateRange dateRange, string symbol, 
        DayOfWeek sourceDay, DayFilter sourceDirection, DayOfWeek followDay)
    {
        DateRange = dateRange;
        Symbol = symbol;
        SourceDay = sourceDay;
        SourceDirection = sourceDirection;
        FollowDay = followDay;
    }

    public DateRange DateRange { get; init; } 
    
    public string Symbol { get; init; } 
    
    public DayOfWeek SourceDay { get; init; } 
    
    public DayFilter SourceDirection { get; init; } 
    
    public DayOfWeek FollowDay { get; init; }

    public override string ToString()
    {
        return DateRange + $"Symbol: {Symbol}" + $"When {SourceDay} is {SourceDirection} FollowDay: {FollowDay}";
    }
}