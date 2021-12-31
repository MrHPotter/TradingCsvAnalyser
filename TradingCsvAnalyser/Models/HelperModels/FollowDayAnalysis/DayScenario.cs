using System;
using TradingCsvAnalyser.Models.Enums;

namespace TradingCsvAnalyser.Models.HelperModels;

public class DayScenario
{
    public DayScenario(DayOfWeek dayOfWeek, DayFilter direction)
    {
        DayOfWeek = dayOfWeek;
        Direction = direction;
    }
    public DayOfWeek DayOfWeek { get; set; } 
    public DayFilter Direction { get; set; }

    public override string ToString()
    {
        return $"{DayOfWeek} : {Direction}";
    }
}