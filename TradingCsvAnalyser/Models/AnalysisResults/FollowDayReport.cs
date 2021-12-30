using System;
using TradingCsvAnalyser.Models.Enums;
using TradingCsvAnalyser.Models.HelperModels;
using TradingCsvAnalyser.Models.HelperModels.FollowDayAnalysis;

namespace TradingCsvAnalyser.Models.AnalysisResults;

public class FollowDayReport
{
    public FollowDayReport(decimal upDayChance, decimal averageGain, decimal averageRange, FollowDayParameters parameters, params object[] objects)
    {
        UpDayChance = upDayChance;
        AverageGain = averageGain;
        AverageRange = averageRange;
        Parameters = parameters;
        AdditionalData = new();
        if(objects.Length > 0)
            AdditionalData.AddNew(objects);
    }

    public decimal UpDayChance { get; set; }
    
    public decimal AverageGain { get; set; } 
    
    public decimal AverageRange { get; set; } 
    
    public FollowDayParameters Parameters { get; set; } 
    
    public AdditionalData AdditionalData { get; set; } 
    
}