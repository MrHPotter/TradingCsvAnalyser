using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualBasic.CompilerServices;

namespace TradingCsvAnalyser.Models.AnalysisResults.FollowDayReport;

public class FollowDayReportStrings
{
    /// <summary>
    /// For Csv Ex- and Imports
    /// </summary>
    public FollowDayReportStrings(){}

    /// <summary>
    /// For Saving the Session
    /// </summary>
    /// <param name="report"></param>
    public FollowDayReportStrings(FollowDayReport report)
    {
        UpDayChance = report.UpDayChance.ToString("0.######");

        AverageGain = report.AverageGain.ToString("0.######");

        AverageRange = report.AverageRange.ToString("0.######");

        Parameters = report.Parameters.ToString();

        AdditionalData = report.AdditionalData.ToString();
        
        Creation = DateTime.Now;
    }
    public long Id { get; set; } = default;
    public string UpDayChance { get; set; }

    public string AverageGain { get; set; } 
    
    public string AverageRange { get; set; } 
    
    public string Parameters { get; set; } 
    
    public string AdditionalData { get; set; }
    
    public DateTime Creation { get; set; } 

    [return: NotNullIfNotNull("report")]
    public static implicit operator FollowDayReportStrings?(FollowDayReport? report)
    {
        if (report is null) return null;
        return new(report);
    }
}