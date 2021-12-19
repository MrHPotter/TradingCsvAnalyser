using Microsoft.EntityFrameworkCore;
using TradingCsvAnalyser.DataProviders.Repositories;
using TradingCsvAnalyser.Models.Database;

namespace TradingCsvAnalyser.DataProviders;

public class UnitOfWork: IUnitOfWork
{
    private readonly AnalyserContext _context;
    public IPriceEntryRepository PriceEntryRepository { get; }
    public UnitOfWork(AnalyserContext context)
    {
        _context = context;
        PriceEntryRepository = new PriceEntryRepository(_context);
    }



    public void SaveChanges()
    {
        _context.SaveChanges();
    }
}