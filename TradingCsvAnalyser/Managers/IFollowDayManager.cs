using TradingCsvAnalyser.Models.AnalysisResults;
using TradingCsvAnalyser.Models.HelperModels;

namespace TradingCsvAnalyser.Managers;

public interface IFollowDayManager
{
    public FollowDayReport GetFollowDayReport(FollowDayParameters parameters);
}