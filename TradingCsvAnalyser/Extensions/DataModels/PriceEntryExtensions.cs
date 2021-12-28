using System;
using System.Linq;
using TradingCsvAnalyser.Models;
using TradingCsvAnalyser.Models.AnalysisResults;
using TradingCsvAnalyser.Models.Enums;
using TradingCsvAnalyser.Models.HelperModels;

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
        return entries.AsEnumerable().Where(e => e.Day == weekDay).AsQueryable();
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

    public static DayOfWeekData GetUpDayRatioPerDay(this IQueryable<PriceEntry> entries)
    {
        DayOfWeekData data = new();
        foreach (var day in entries.AsEnumerable().GroupBy(e => e.Day))
        {
            decimal upDays = day.Count(i => i.Close > i.Open);
            decimal downDays = day.Count(i => i.Open > i.Close);
            data.AddDay(day.Key, upDays / (downDays+upDays));
        }

        return data;
    }

    public static IQueryable<PriceEntry> FilterByDayResult(this IQueryable<PriceEntry> entries, DayFilter dayFilter) =>
        dayFilter switch
        {
            DayFilter.None => entries,
            DayFilter.DownDay => entries.OnlyDownDays(),
            DayFilter.UpDay => entries.OnlyUpDays(),
            _ => throw new ArgumentException($"Unavailable DayFilter {dayFilter}")
        };

    private static IQueryable<PriceEntry> OnlyUpDays(this IQueryable<PriceEntry> entries)
    {
        return entries.Where(e => e.Close > e.Open);
    }

    private static IQueryable<PriceEntry> OnlyDownDays(this IQueryable<PriceEntry> entries)
    {
        return entries.Where(e => e.Open > e.Close);
    }

    public static IQueryable<PriceEntry> FilterByDateRange(this IQueryable<PriceEntry> entries, DateRange dateRange)
    {
        return entries.Where(e => e.DateAndTime >= (dateRange.Start ?? DateTime.MinValue) &&
                                  e.DateAndTime <= (dateRange.End ?? DateTime.MaxValue));
    }
}