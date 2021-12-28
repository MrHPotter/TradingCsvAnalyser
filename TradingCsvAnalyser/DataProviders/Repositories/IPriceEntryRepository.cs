using System;
using System.Collections.Generic;
using System.Linq;
using TradingCsvAnalyser.Models;

namespace TradingCsvAnalyser.DataProviders.Repositories;

public interface IPriceEntryRepository
{
    IQueryable<PriceEntry> GetAllEntries();
    void AddNewEntries(IEnumerable<PriceEntry> entries);
    IQueryable<PriceEntry> GetEntriesForSymbol(string symbol);
    IQueryable<PriceEntry> GetEntriesForDay(string symbol, DayOfWeek dayOfWeek);
    IEnumerable<string> GetAvailableSymbols();
}