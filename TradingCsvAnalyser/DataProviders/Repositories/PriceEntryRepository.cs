using System;
using System.Collections.Generic;
using System.Linq;
using TradingCsvAnalyser.Models;
using TradingCsvAnalyser.Models.Database;

namespace TradingCsvAnalyser.DataProviders.Repositories;

public class PriceEntryRepository : IPriceEntryRepository
{
    private AnalyserContext _context;
    public PriceEntryRepository(AnalyserContext context)
    {
        _context = context;
    }

    public IEnumerable<PriceEntry> GetAllEntries()
    {
        return _context.PriceEntries;
    }

    public void AddNewEntries(IEnumerable<PriceEntry> entries)
    {
        _context.PriceEntries.AddRange(entries.Where(i => !_context.PriceEntries.Contains(i)));
    }

    public IEnumerable<PriceEntry> GetEntriesForSymbol(string symbol)
    {
        return _context.PriceEntries.Where(i => i.Symbol.ToLower() == symbol.ToLower());
    }

    public IEnumerable<PriceEntry> GetEntriesInTimeRange(DateTime start, DateTime end)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<PriceEntry> GetTimeEntriesForDay(DayOfWeek dayOfWeek)
    {
        throw new NotImplementedException();
    }
}