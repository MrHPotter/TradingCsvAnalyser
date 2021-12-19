using TradingCsvAnalyser.Models.AnalysisResults;
using TradingCsvAnalyser.Models.Enums;

namespace TradingCsvAnalyser.Managers;

public interface IAggregationManager
{
    public DayOfWeekData GetAverageRangePerDay(CandleRange rangeType);
}