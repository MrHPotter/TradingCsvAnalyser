using System;
using Microsoft.Win32;
using TradingCsvAnalyser.Models.Enums;


namespace TradingCsvAnalyser.Models;

public class PriceEntry
{
    public PriceEntry(DateTimeOffset dateAndTime, decimal open, decimal high, 
        decimal low, decimal close, DayOfWeek day, DateOnly date, string symbol)
    {
        DateAndTime = dateAndTime;
        Open = open;
        High = high;
        Low = low;
        Close = close;
        Day = day;
        Date = date;
        Symbol = symbol;
    }

    public DateTimeOffset DateAndTime { get; set; }
    public decimal Open { get; set; }
    public decimal High { get; set; }
    public decimal Low { get; set; }
    public decimal Close { get; set; }
    public DayOfWeek Day { get; set; }
    public DateOnly Date { get; set; }
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

    private decimal OpenCloseRange() =>  Close - Open;
    private decimal HighLowRange() => High - Low;
    private decimal OpenHighRange() => High-Open;
    private decimal OpenLowRange() => Open - Low;
    private decimal LowCloseRange() => Close - Low;
    private decimal HighCloseRange() => High - Close;
}