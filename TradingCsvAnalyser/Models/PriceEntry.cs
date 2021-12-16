using System;

namespace TradingCsvAnalyser.Models;

public class PriceEntry
{
    public PriceEntry(DateTimeOffset dateAndTime, decimal open, decimal high, 
        decimal low, decimal close, byte day, DateOnly date, string symbol)
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
    public byte Day { get; set; }
    public DateOnly Date { get; set; }
    public string Symbol { get; set; }
}