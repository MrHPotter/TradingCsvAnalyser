using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using TradingCsvAnalyser.DataProviders.AnalysisResults;
using TradingCsvAnalyser.Models;
namespace TradingCsvAnalyser.Extensions;

public static class PriceEntryExtensions
{
    public static IEnumerable<PriceEntry> FilterForSymbol(this IEnumerable<PriceEntry> entries, string symbol)
    {
        return entries.Where(i => i.Symbol.ToLower() == symbol.ToLower());
    }

    public static IEnumerable<PriceEntry> FilterForTimeRange(this IEnumerable<PriceEntry> entries, DateTime start,
        DateTime end)
    {
        return entries.Where(e => e.DateAndTime >= start && e.DateAndTime <= end);
    }

    public static IEnumerable<PriceEntry> FilterForDayOfWeek(this IEnumerable<PriceEntry> entries, DayOfWeek weekDay)
    {
        return entries.Where(e => e.Day == weekDay);
    }

    public static DayOfWeekData GetAveragePerDay(this IEnumerable<PriceEntry> entries, Func<PriceEntry, decimal> selector)
    {
        DayOfWeekData data = new();
        foreach (var day in entries.GroupBy(e => e.Day))
        {
            data.AddDay(day.Key, day.Average(selector));
        }

        return data;
    }
}