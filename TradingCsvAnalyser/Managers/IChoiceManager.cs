using System;
using System.Collections.Generic;
using TradingCsvAnalyser.Models.Enums;

namespace TradingCsvAnalyser.Managers;

public interface IChoiceManager
{
    public IEnumerable<CandleRange> GetAvailableCandleRanges();

    public IEnumerable<string> GetAvailableSymbols();

    public IEnumerable<string> GetAvailableMethods();

    public IEnumerable<DayOfWeek> GetValidDays();

    public IEnumerable<DayFilter> GetValidDirections();

}