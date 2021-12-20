using System;
using TradingCsvAnalyser.DataProviders;
using TradingCsvAnalyser.Extensions;
using TradingCsvAnalyser.Extensions.DataModels;
using TradingCsvAnalyser.Models.AnalysisResults;
using TradingCsvAnalyser.Models.Enums;

namespace TradingCsvAnalyser.Managers;

public class AggregationManager : IAggregationManager
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

    public DayOfWeekData GetAverageRangePerDay(CandleRange rangeType, string symbol)
    {
        return _data.PriceEntryRepository.GetEntriesForSymbol(symbol)
            .GetAveragePerDay(i => i.Range(rangeType));
    }

    public DayOfWeekData GetSumRangePerDay(CandleRange rangeType, string symbol)
    {
        return _data.PriceEntryRepository.GetEntriesForSymbol(symbol)
            .GetSumPerDay(i => i.Range(rangeType));
    }

    public DayOfWeekData CallMethodByName(string method, CandleRange rangeType, string symbol)
    {
        switch (method)
        {
            case nameof(GetAverageRangePerDay):
                return GetAverageRangePerDay(rangeType, symbol);
            case nameof(GetSumRangePerDay):
                return GetSumRangePerDay(rangeType, symbol);
            default:
                throw new ArgumentException($"{method} is not a valid Method");
        }
    }
}