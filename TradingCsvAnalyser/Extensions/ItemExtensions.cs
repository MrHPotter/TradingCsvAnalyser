using System;
using TradingCsvAnalyser.Models.Enums;

namespace TradingCsvAnalyser.Extensions;

public static class ItemExtensions
{
    public static DayOfWeek? GetDayOfWeek(this object? obj)
    {
        var result = Enum.TryParse<DayOfWeek>(obj?.ToString(), out var dayOfWeek);
        if(result)
            return dayOfWeek;
        return null;
    }

    public static DayFilter? GetDayFilter(this object? obj)
    {
        var result = Enum.TryParse<DayFilter>(obj?.ToString(), out var dayFilter);
        if(result)
            return dayFilter;
        return null;
    }
}