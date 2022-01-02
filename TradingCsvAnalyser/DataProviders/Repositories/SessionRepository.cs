using System.Collections.Generic;
using System.Collections.ObjectModel;
using TradingCsvAnalyser.Models.AnalysisResults;
using TradingCsvAnalyser.Models.Database;
using TradingCsvAnalyser.Models.HelperModels.FollowDayAnalysis;

namespace TradingCsvAnalyser.DataProviders.Repositories;

public class SessionRepository : ISessionRepository
{
    private SessionContext _context;

    public SessionRepository(SessionContext context)
    {
        _context = context;
    }

    public void SaveSession<TEntity>(ObservableCollection<TEntity> collection) where TEntity : class
    {
        var set = _context.Set<TEntity>();
        set.RemoveRange(set);
        _context.SaveChanges();
        set.AddRange(collection);
        _context.SaveChanges();
    }
    

    public IEnumerable<TEntity> GetSession<TEntity>() where TEntity : class
    {
        return _context.Set<TEntity>();
    }
}