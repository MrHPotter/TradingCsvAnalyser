using System;
using TradingCsvAnalyser.Models.AnalysisResults;
using TradingCsvAnalyser.Models.Enums;
using TradingCsvAnalyser.Models.HelperModels;

namespace TradingCsvAnalyser.Managers;

public interface IDayOfWeekDataManager
{
    public DayOfWeekData GetAverageRangePerDay(DoWDefaultParameters parameters);

    public DayOfWeekData GetSumRangePerDay(DoWDefaultParameters parameters);
    public DayOfWeekData GetUpDayRatioPerDay(string symbol, DateRange dateRange);
    public DayOfWeekData CallMethodByName(string method, DoWDefaultParameters doWDefaultParameters);
}