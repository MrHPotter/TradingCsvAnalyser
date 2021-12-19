using System;

namespace TradingCsvAnalyser.Models.SourceModels;

public class PriceDownload
{
    public DateTimeOffset Date { get; set; }
    public decimal Price { get; set; }
    public decimal Open { get; set; }
    public decimal High { get; set; }
    public decimal Low { get; set; }
}