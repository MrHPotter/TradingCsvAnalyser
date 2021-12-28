using System.Linq;
using TradingCsvAnalyser.DataProviders;
using TradingCsvAnalyser.Extensions.DataModels;
using TradingCsvAnalyser.Models;
using TradingCsvAnalyser.Models.AnalysisResults;
using TradingCsvAnalyser.Models.HelperModels;

namespace TradingCsvAnalyser.Managers;

public class FollowDayManager : IFollowDayManager
{
    private readonly IUnitOfWork _data;

    public FollowDayManager(IUnitOfWork data)
    {
        _data = data;
    }

    public FollowDayReport GetFollowDayReport(FollowDayParameters parameters)
    {
        const decimal upDayThreshhold = 0;
        decimal totalRangeFollow = 0;
        decimal totalGainFollow = 0;
        decimal upDaysCountFollow = 0;
        
        var sourceDays = GetSourceEntries(parameters);
        
        decimal totalDaysCount = sourceDays.Count();
        
        var followDays = GetFollowEntries(parameters);
        
        foreach (var sourceDay in sourceDays)
        {
            var followDay = followDays.FirstOrDefault(d => d.DateAndTime > sourceDay.DateAndTime);
            if (followDay is null)
            {
                totalDaysCount = -1;
                break;
            }

            if (followDay.OpenCloseRange() > upDayThreshhold)
                upDaysCountFollow++;
            
            totalRangeFollow += followDay.HighLowRange();
            totalGainFollow += followDay.OpenCloseRange();

        }

        var upDayRatioFollow = upDaysCountFollow / totalDaysCount;
        var averageRangeFollow = totalRangeFollow / totalDaysCount;
        var averageGainFollow = totalGainFollow / totalDaysCount;
        return new FollowDayReport(upDayRatioFollow, averageGainFollow, averageRangeFollow, parameters);
    }

    private IQueryable<PriceEntry> GetSourceEntries(FollowDayParameters parameters)
    {
        return _data.PriceEntryRepository
            .GetEntriesForDay(parameters.Symbol, parameters.SourceDay)
            .FilterByDateRange(parameters.DateRange)
            .FilterByDayResult(parameters.SourceDirection);
    }

    private IOrderedQueryable<PriceEntry> GetFollowEntries(FollowDayParameters parameters)
    {
        // To be decided wether we want to Filter by Date Range here
        return _data.PriceEntryRepository
            .GetEntriesForDay(parameters.Symbol, parameters.FollowDay)
            .FilterByDateRange(parameters.DateRange)
            .OrderBy(f => f.DateAndTime);
    }
}