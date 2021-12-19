using System;
using System.Collections.Generic;
using TradingCsvAnalyser.Models;

namespace TradingCsvAnalyser.DataProviders.Repositories;

public interface IPriceEntryRepository
{
    IEnumerable<PriceEntry> GetAllEntries();
    void AddNewEntries(IEnumerable<PriceEntry> entries);
    IEnumerable<PriceEntry> GetEntriesForSymbol(string symbol);
    IEnumerable<PriceEntry> GetEntriesInTimeRange(DateTime start, DateTime end);
    IEnumerable<PriceEntry> GetTimeEntriesForDay(DayOfWeek dayOfWeek);
}