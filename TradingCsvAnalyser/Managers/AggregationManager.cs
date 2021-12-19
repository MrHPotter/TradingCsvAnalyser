﻿using TradingCsvAnalyser.DataProviders;
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
}