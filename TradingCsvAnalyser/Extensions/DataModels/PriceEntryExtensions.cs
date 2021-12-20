﻿using System;
using System.Linq;
using TradingCsvAnalyser.Models;
using TradingCsvAnalyser.Models.AnalysisResults;

namespace TradingCsvAnalyser.Extensions.DataModels;

public static class PriceEntryExtensions
{
    public static IQueryable<PriceEntry> FilterForSymbol(this IQueryable<PriceEntry> entries, string symbol)
    {
        return entries.Where(i => i.Symbol.ToLower() == symbol.ToLower());
    }

    public static IQueryable<PriceEntry> FilterForTimeRange(this IQueryable<PriceEntry> entries, DateTime start,
        DateTime end)
    {
        return entries.Where(e => e.DateAndTime >= start && e.DateAndTime <= end);
    }

    public static IQueryable<PriceEntry> FilterForDayOfWeek(this IQueryable<PriceEntry> entries, DayOfWeek weekDay)
    {
        return entries.Where(e => e.Day == weekDay);
    }

    public static DayOfWeekData GetAveragePerDay(this IQueryable<PriceEntry> entries, Func<PriceEntry, decimal> selector)
    {
        DayOfWeekData data = new();
        foreach (var day in entries.AsEnumerable().GroupBy(e => e.Day))
        {
            data.AddDay(day.Key, day.Average(selector));
        }

        return data;
    }

    public static DayOfWeekData GetSumPerDay(this IQueryable<PriceEntry> entries, Func<PriceEntry, decimal> selector)
    {
        DayOfWeekData data = new();
        foreach (var day in entries.AsEnumerable().GroupBy(e => e.Day))
        {
            data.AddDay(day.Key, day.Sum(selector));
        }

        return data;
    }
}