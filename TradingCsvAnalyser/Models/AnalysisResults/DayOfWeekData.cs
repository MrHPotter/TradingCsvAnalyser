using System;

namespace TradingCsvAnalyser.Models.AnalysisResults;

public class DayOfWeekData
{
    public DayOfWeekData()
    {
        Monday = null;
        Tuesday = null;
        Wednesday = null;
        Thursday = null;
        Friday = null;
    }
    public decimal? Monday { get; private set; }
    public decimal? Tuesday { get; private set; }
    public decimal? Wednesday { get; private set; }
    public decimal? Thursday { get; private set; }
    public decimal? Friday { get; private set; }

    public DayOfWeekData AddDay(DayOfWeek day, decimal value)
    {
        switch (day)
        {
            case DayOfWeek.Monday:
                Monday = value;
                break;
            case DayOfWeek.Tuesday:
                Tuesday = value;
                break;
            case DayOfWeek.Wednesday:
                Wednesday = value;
                break;
            case DayOfWeek.Thursday:
                Thursday = value;
                break;
            case DayOfWeek.Friday:
                Friday = value;
                break;
            case DayOfWeek.Sunday:
            case DayOfWeek.Saturday:
            default:
                break;
        }

        return this;
    }
}