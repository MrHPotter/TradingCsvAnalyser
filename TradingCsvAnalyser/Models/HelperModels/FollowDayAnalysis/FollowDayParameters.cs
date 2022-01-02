using System;
using System.Collections.Generic;
using System.Linq;
using TradingCsvAnalyser.Models.Enums;

namespace TradingCsvAnalyser.Models.HelperModels.FollowDayAnalysis;

public class FollowDayParameters
{
    public FollowDayParameters(){}
    public FollowDayParameters(DateRange dateRange, string symbol,DayScenario sourceDay, DayOfWeek followDay, IEnumerable<DayScenario>? scenario)
    {
        DateRange = dateRange;
        Symbol = symbol;
        FollowDay = followDay;
        SourceDay = sourceDay;
        Scenario = scenario?.ToList() ?? new();
    }

    public DateRange DateRange { get; init; } 
    
    public string Symbol { get; init; } 
    
    public DayScenario SourceDay { get; set; } 
    public List<DayScenario> Scenario { get; set; } 
    
    public DayOfWeek FollowDay { get; init; }

    public override string ToString()
    {
        return DateRange + $"Symbol: {Symbol}" + 
               $"Scenario:[{string.Join('|',Scenario.Select(i => i.ToString()))}] FollowDay: {FollowDay}";
    }
}