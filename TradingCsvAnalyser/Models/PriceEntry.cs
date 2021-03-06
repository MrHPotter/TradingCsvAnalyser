using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Win32;
using TradingCsvAnalyser.Extensions;
using TradingCsvAnalyser.Extensions.BaseTypeExtensions;
using TradingCsvAnalyser.Extensions.DataModels;
using TradingCsvAnalyser.Models.Enums;
using TradingCsvAnalyser.Models.HelperModels;
using TradingCsvAnalyser.Models.HelperModels.FollowDayAnalysis;
using TradingCsvAnalyser.Models.SourceModels;


namespace TradingCsvAnalyser.Models;

public class PriceEntry
{
    public PriceEntry()
    {
        
    }
    public PriceEntry(DateTimeOffset dateAndTime, decimal open, decimal high, 
        decimal low, decimal close,  string symbol)
    {
        DateAndTime = dateAndTime.LocalDateTime;
        Open = open;
        High = high;
        Low = low;
        Close = close;
        Symbol = symbol;
    }

    public PriceEntry(PriceDownload input, string symbol)
    {
        DateAndTime = input.Date.LocalDateTime;
        Close = input.Price;
        Open = input.Open;
        High = input.High;
        Low = input.Low;
        Symbol = symbol;
    }

    public DateTime DateAndTime { get; set; }
    public decimal Open { get; set; }
    public decimal High { get; set; }
    public decimal Low { get; set; }
    public decimal Close { get; set; }
    public DayOfWeek Day => DateAndTime.DayOfWeek;
    public DateOnly Date => DateAndTime.DateOnly();
    public string Symbol { get; set; }

    public override bool Equals(object? obj)
    {
        return Equals(obj as PriceEntry);
    }

    private bool Equals(PriceEntry? entry)
    {
        if (entry is null)
            return false;
        
        if (ReferenceEquals(this, entry)) return true;
        
        return entry.Close == Close &&
               entry.Date == Date &&
               entry.Day == Day &&
               entry.High == High &&
               entry.Low == Low &&
               entry.Open == Open &&
               entry.Symbol == Symbol &&
               entry.DateAndTime == DateAndTime;
    }

    public decimal Range(CandleRange rangeType) => rangeType switch
    {
        CandleRange.HighClose => HighCloseRange(),
        CandleRange.HighLow => HighLowRange(),
        CandleRange.LowClose => LowCloseRange(),
        CandleRange.OpenClose => OpenCloseRange(),
        CandleRange.OpenHigh => OpenHighRange(),
        CandleRange.OpenLow => OpenLowRange(),
        _ => throw new ArgumentOutOfRangeException(nameof(rangeType), $"Unexpected value for Range Type: {rangeType}")
    };

    public DayFilter Direction(decimal threshold)
    {
        if (threshold < OpenCloseRange())
            return DayFilter.UpDay;
        if (-threshold> OpenCloseRange())
            return DayFilter.DownDay;
        return DayFilter.None;
    }

    public bool MatchesScenario(IEnumerable<DayScenario> scenario, IQueryable<PriceEntry> allEntries)
    {
        if (!scenario.Any()) return true;
        foreach (var dayScenario in scenario)
        {
            var relevantDay = allEntries
                .FilterForDayOfWeek(dayScenario.DayOfWeek)
                .OrderByDescending(d => d.Date)
                .FirstOrDefault(p => p.Date< Date);
            if (relevantDay is null || relevantDay.Direction(0) != dayScenario.Direction)
                return false;
        }

        return true;
    }
    public decimal OpenCloseRange() =>  Close - Open;
    public decimal HighLowRange() => High - Low;
    public decimal OpenHighRange() => High-Open;
    public decimal OpenLowRange() => Open - Low;
    public decimal LowCloseRange() => Close - Low;
    public decimal HighCloseRange() => High - Close;

    public static implicit operator PriceEntry?(PriceDownloadWithSymbol? download)
    {
        if (download is null) return null;
        return new(download, download.Symbol);
    }
}