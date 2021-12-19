using System.Collections.Generic;
using System.Linq;
using TradingCsvAnalyser.Models;
using TradingCsvAnalyser.Models.SourceModels;

namespace TradingCsvAnalyser.Extensions.DataModels;

public static class PriceDownloadExtensions
{
    public static IEnumerable<PriceDownloadWithSymbol> AddSymbol(this IEnumerable<PriceDownload> downloads,
        string symbol)
    {
        return downloads.Select(download => new PriceDownloadWithSymbol(download, symbol));
    }
    
    public static IEnumerable<PriceEntry> ToPriceEntries(this IEnumerable<PriceDownload> downloads,
        string symbol)
    {
        return downloads.Select(download => new PriceEntry(download, symbol));
    }
}