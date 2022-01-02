using System;
using System.Diagnostics.CodeAnalysis;

namespace TradingCsvAnalyser.Models.AnalysisResults.DayOfWeekAnalysis;

public class DoWOverviewStrings
{
    /// <summary>
    /// For Csv Importing
    /// </summary>
    public DoWOverviewStrings(){}

    /// <summary>
    /// For Exporting a Report or Saving the Session
    /// </summary>
    /// <param name="doWOverview"></param>
    public DoWOverviewStrings(DoWOverview doWOverview)
    {
        Monday = doWOverview.Monday?.ToString("0.######") ?? "";
        Tuesday = doWOverview.Tuesday?.ToString("0.######") ?? "";
        Wednesday = doWOverview.Wednesday?.ToString("0.######") ?? "";
        Thursday = doWOverview.Thursday?.ToString("0.######") ?? "";
        Friday = doWOverview.Friday?.ToString("0.######") ?? "";

        Symbol = doWOverview.Symbol;
        Method = doWOverview.Method;
        Range = doWOverview.Range;
        DayFilter = doWOverview.DayFilter.ToString();
        DateRange = doWOverview.DateRange.ToString();
        
        Creation = DateTime.Now;
    }

    public long Id { get; set; } = default;
    public string Monday { get; set; } 
    public string Tuesday { get; set; }
    public string Wednesday { get; set; }
    public string Thursday { get; set; }
    public string Friday { get; set; }
    
    public string Symbol { get; set; } 
    public string Method { get; set; } 
    public string Range { get; set; }
    public string DayFilter { get; set; }
    public string DateRange { get; set; } 
    
    public DateTime Creation { get; set; } 
    
    [return: NotNullIfNotNull("overview")]
    public static implicit operator DoWOverviewStrings?(DoWOverview? overview)
    {
        if (overview is null) return null;
        return new(overview);
    }
}