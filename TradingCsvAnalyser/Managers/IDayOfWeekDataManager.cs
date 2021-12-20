using TradingCsvAnalyser.Models.AnalysisResults;
using TradingCsvAnalyser.Models.Enums;

namespace TradingCsvAnalyser.Managers;

public interface IDayOfWeekDataManager
{
    public DayOfWeekData GetAverageRangePerDay(CandleRange rangeType);
    
    public DayOfWeekData GetAverageRangePerDay(CandleRange rangeType, string symbol);

    public DayOfWeekData GetSumRangePerDay(CandleRange rangeType, string symbol);

    public DayOfWeekData CallMethodByName(string method, CandleRange rangeType, string symbol);
}