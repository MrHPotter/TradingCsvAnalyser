using System;
using System.Collections.Generic;
using System.Linq;
using TradingCsvAnalyser.Extensions;
using TradingCsvAnalyser.Extensions.DataModels;
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

    public IQueryable<PriceEntry> GetAllEntries()
    {
        return _context.PriceEntries;
    }

    public void AddNewEntries(IEnumerable<PriceEntry> entries)
    {
        _context.PriceEntries.AddRange(entries.Where(i => !_context.PriceEntries.Contains(i)));
    }

    public IQueryable<PriceEntry> GetEntriesForSymbol(string symbol)
    {
        return GetAllEntries().Where(i => i.Symbol.ToLower() == symbol.ToLower());
    }

    public IQueryable<PriceEntry> GetEntriesForDay(string symbol, DayOfWeek dayOfWeek)
    {
        return GetEntriesForSymbol(symbol).FilterForDayOfWeek(dayOfWeek);
    }

    public IEnumerable<string> GetAvailableSymbols()
    {
        return _context.PriceEntries.AsEnumerable().DistinctBy(e => e.Symbol).Select(p => p.Symbol);
    }
}