using TradingCsvAnalyser.DataProviders.Repositories;

namespace TradingCsvAnalyser.DataProviders;

public interface IUnitOfWork
{
    public IPriceEntryRepository PriceEntryRepository { get; }
    public void SaveChanges();
}