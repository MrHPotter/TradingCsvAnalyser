using System;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace TradingCsvAnalyser.Models.HelperModels;

public class DateRange
{
    public DateRange()
    {
        Start = null;
        End = null;
    }
    public DateRange(DateTime? start, DateTime? end)
    {
        Start = start;
        End = end;
    }
    public DateTime? Start { get; set; }
    public DateTime? End { get; set; }

    public override string ToString()
    {
        if (Start is null && End is null)
            return "All";
        if (Start is null)
            return $"Until {End!.Value.ToShortDateString()}";
        if (End is null)
            return $"From {Start!.Value.ToShortDateString()}";
        return $"{Start!.Value.ToShortDateString()} to {End!.Value.ToShortDateString()}";
    }
}