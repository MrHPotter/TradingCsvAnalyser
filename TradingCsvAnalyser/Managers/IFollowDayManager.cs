using TradingCsvAnalyser.Models.AnalysisResults;
using TradingCsvAnalyser.Models.HelperModels;
using TradingCsvAnalyser.Models.HelperModels.FollowDayAnalysis;

namespace TradingCsvAnalyser.Managers;

public interface IFollowDayManager
{
    public FollowDayReport GetFollowDayReport(FollowDayParameters parameters);
}