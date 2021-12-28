using System;
using TradingCsvAnalyser.Extensions;

namespace TradingCsvAnalyser.Models;

public class PriceEntryResult
{
    public PriceEntryResult(PriceEntry priceEntry)
    {
        DateAndTime = priceEntry.DateAndTime;
        Open = priceEntry.Open;
        High = priceEntry.High;
        Low = priceEntry.Low;
        Close = priceEntry.Close;
        Symbol = priceEntry.Symbol;
        OpenCloseRange = Close - Open;
        HighLowRange = High - Low;
        OpenHighRange = High - Open;
        OpenLowRange = Open - Low;
        LowCloseRange = Close - Low;
        HighCloseRange = High - Close;
    }
    public DateTime DateAndTime { get; set; }
    public decimal Open { get; set; }
    public decimal High { get; set; }
    public decimal Low { get; set; }
    public decimal Close { get; set; }
    
    public DayOfWeek Day => DateAndTime.DayOfWeek;
    public DateOnly Date => DateAndTime.DateOnly();
    public string Symbol { get; set; }
    
    public decimal OpenCloseRange { get; set; }
    public decimal HighLowRange { get; set; }
    public decimal OpenHighRange { get; set; }
    public decimal OpenLowRange { get; set; }
    public decimal LowCloseRange { get; set; }
    public decimal HighCloseRange { get; set; }

    public static implicit operator PriceEntryResult?(PriceEntry? priceEntry)
    {
        if (priceEntry is null) return null;
        return new PriceEntryResult(priceEntry);
    }
    
}