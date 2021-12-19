using Microsoft.EntityFrameworkCore;
using TradingCsvAnalyser.DataProviders.Repositories;
using TradingCsvAnalyser.Models.Database;

namespace TradingCsvAnalyser.DataProviders;

public class UnitOfWork: IUnitOfWork
{
    private readonly AnalyserContext _context;
    public IPriceEntryRepository PriceEntryRepository { get; }
    public UnitOfWork(IDbContextFactory<AnalyserContext> contextFactory)
    {
        _context = contextFactory.CreateDbContext();
        PriceEntryRepository = new PriceEntryRepository(_context);
    }



    public void SaveChanges()
    {
        _context.SaveChanges();
    }
}