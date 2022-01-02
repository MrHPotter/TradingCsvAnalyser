using System.Collections.Generic;
using System.Collections.ObjectModel;
using TradingCsvAnalyser.Models.HelperModels.FollowDayAnalysis;

namespace TradingCsvAnalyser.DataProviders.Repositories;

public interface ISessionRepository
{
    public void SaveSession<TEntity>(ObservableCollection<TEntity> collection) where TEntity : class;

    public IEnumerable<TEntity> GetSession<TEntity>() where TEntity : class;
}