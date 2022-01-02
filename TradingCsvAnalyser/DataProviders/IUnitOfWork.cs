using TradingCsvAnalyser.DataProviders.Repositories;

namespace TradingCsvAnalyser.DataProviders;

public interface IUnitOfWork
{
    public IPriceEntryRepository PriceEntryRepository { get; }
    public ISessionRepository SessionRepository { get; }
    public void SaveAnalyserChanges();

    public void SaveSessionChanges();
}