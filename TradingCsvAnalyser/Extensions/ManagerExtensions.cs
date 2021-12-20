﻿using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using TradingCsvAnalyser.Models.AnalysisResults;

namespace TradingCsvAnalyser.Extensions;

public static class ManagerExtensions
{
    public static string[] GetDayOfWeekMethods<T>(this T manager)
    {
        return manager is null ? Array.Empty<string>() : 
            manager.GetType().Methods().ThatReturn<DayOfWeekData>().Where(m => m.GetParameters().Length == 2)
                .Select(i => i.Name).ToArray();
    }
}