using System;
using System.Collections.Generic;
using System.Linq;
using TradingCsvAnalyser.DataProviders;
using TradingCsvAnalyser.Extensions.DataModels;
using TradingCsvAnalyser.Models;
using TradingCsvAnalyser.Models.AnalysisResults;
using TradingCsvAnalyser.Models.HelperModels;
using TradingCsvAnalyser.Models.HelperModels.FollowDayAnalysis;

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
        
        var sourceDays = GetSourceEntries(parameters).ToList();
        
        decimal totalDaysCount = sourceDays.Count();
        
        var potentialFollowDays = GetFollowEntries(parameters).ToList();
        List<PriceEntry> actualFollowDays = new();
        
        foreach (var sourceDay in sourceDays)
        {
            var followDay = potentialFollowDays.FirstOrDefault(d => d.DateAndTime > sourceDay.DateAndTime);
            if (followDay is null)
            {
                totalDaysCount -= 1;
                break;
            }

            if (followDay.OpenCloseRange() > upDayThreshhold)
                upDaysCountFollow++;
            
            totalRangeFollow += followDay.HighLowRange();
            totalGainFollow += followDay.OpenCloseRange();
            actualFollowDays.Add(followDay);
        }

        
        var averageOpenLowFollow = actualFollowDays.Average(p => p.OpenLowRange());
        var maxOpenLowFollow = actualFollowDays.Max(p => p.OpenLowRange());
        var averageOpenHighFollow = actualFollowDays.Average(p => p.OpenHighRange());
        var maxOpenHighFollow = actualFollowDays.Max(p => p.OpenHighRange());
        var info = new object[]
        {
            new {AvgOpenLow = averageOpenLowFollow.ToString("0.######")},
            new {MaxOpenLow = maxOpenLowFollow.ToString("0.##")},
            new {AvgOpenHigh = averageOpenHighFollow.ToString("0.######")},
            new {MaxOpenHigh = maxOpenHighFollow.ToString("0.##")}
        };

        var upDayRatioFollow = upDaysCountFollow / totalDaysCount;
        var averageRangeFollow = totalRangeFollow / totalDaysCount;
        var averageGainFollow = totalGainFollow / totalDaysCount;
        return new FollowDayReport(upDayRatioFollow, averageGainFollow, averageRangeFollow, parameters, info);
    }

    private List<PriceEntry> GetFollowDays(FollowDayParameters parameters)
    {
        var sourceDays = GetSourceEntries(parameters).ToList(); 
        var potentialFollowDays = GetFollowEntries(parameters).ToList();
        List<PriceEntry> actualFollowDays = new();
        foreach (var priceEntry in sourceDays)
        {
            var followDay = potentialFollowDays.FirstOrDefault(d => d.DateAndTime > priceEntry.DateAndTime);
            if (followDay is null) break;
            actualFollowDays.Add(followDay);
        }
        return actualFollowDays;
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