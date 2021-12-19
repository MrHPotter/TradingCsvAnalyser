namespace TradingCsvAnalyser.Models.SourceModels;

public class PriceDownloadWithSymbol : PriceDownload
{
    public PriceDownloadWithSymbol(PriceDownload download, string symbol)
    {
        Date = download.Date;
        Price = download.Price;
        Open = download.Open;
        High = download.High;
        Low = download.Low;
        Symbol = symbol;
    }
    public string Symbol { get; set; }
}