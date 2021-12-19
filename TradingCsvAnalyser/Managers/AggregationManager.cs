using TradingCsvAnalyser.DataProviders;
using TradingCsvAnalyser.DataProviders.AnalysisResults;
using TradingCsvAnalyser.Extensions;
using TradingCsvAnalyser.Extensions.DataModels;
using TradingCsvAnalyser.Models.Enums;

namespace TradingCsvAnalyser.Managers;

public class AggregationManager
{
    private readonly IUnitOfWork _data;

    public AggregationManager(IUnitOfWork data)
    {
        _data = data;
    }

    public DayOfWeekData GetAverageRangePerDay(CandleRange rangeType)
    {
        return _data.PriceEntryRepository.GetAllEntries()
            .GetAveragePerDay(i => i.Range(rangeType));
    }
}