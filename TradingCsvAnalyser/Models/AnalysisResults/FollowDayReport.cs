using System;
using TradingCsvAnalyser.Models.Enums;
using TradingCsvAnalyser.Models.HelperModels;

namespace TradingCsvAnalyser.Models.AnalysisResults;

public class FollowDayReport
{
    public FollowDayReport(decimal upDayChance, decimal averageGain, decimal averageRange, FollowDayParameters parameters)
    {
        UpDayChance = upDayChance;
        AverageGain = averageGain;
        AverageRange = averageRange;
        Parameters = parameters;
    }

    public decimal UpDayChance { get; set; }
    
    public decimal AverageGain { get; set; } 
    
    public decimal AverageRange { get; set; } 
    
    FollowDayParameters Parameters { get; set; } 
    
}