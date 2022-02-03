using System.Collections.Generic;
using System.Linq;
using TradingCsvAnalyser.Models.AnalysisResults;
using TradingCsvAnalyser.Models.AnalysisResults.DayOfWeekAnalysis;

namespace TradingCsvAnalyser.Extensions.DataModels;

public static class DayOfWeekDataExtensions
{
    public static List<DayOfWeekData> AddNew(this List<DayOfWeekData> collection, DayOfWeekData data)
    {
        if (!collection.Contains(data))
            return collection.Append(data).ToList();
        return collection;
    }
}