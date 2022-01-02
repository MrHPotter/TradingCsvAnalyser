using Microsoft.EntityFrameworkCore;
using TradingCsvAnalyser.DataProviders.Repositories;
using TradingCsvAnalyser.Models.AnalysisResults;
using TradingCsvAnalyser.Models.Database;

namespace TradingCsvAnalyser.DataProviders;

public class UnitOfWork: IUnitOfWork
{
    private readonly AnalyserContext _analyserContext;
    private readonly SessionContext _sessionContext;
    
    public IPriceEntryRepository PriceEntryRepository { get; }
    public ISessionRepository SessionRepository { get; }

    public UnitOfWork(AnalyserContext analyserContext, SessionContext sessionContext)
    {
        _analyserContext = analyserContext;
        _sessionContext = sessionContext;
        SessionRepository = new SessionRepository(_sessionContext);
        PriceEntryRepository = new PriceEntryRepository(_analyserContext);
    }



    public void SaveAnalyserChanges()
    {
        _analyserContext.SaveChanges();
    }

    public void SaveSessionChanges()
    {
        _sessionContext.SaveChanges();
    }
}