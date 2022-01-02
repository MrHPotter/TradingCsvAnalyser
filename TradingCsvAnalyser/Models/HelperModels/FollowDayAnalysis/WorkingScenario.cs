using System;
using System.Collections.Generic;
using System.Linq;

namespace TradingCsvAnalyser.Models.HelperModels.FollowDayAnalysis;

[Obsolete]
public sealed class WorkingScenario
{
    public WorkingScenario(IEnumerable<DayScenario> dayScenarios)
    {
        ScenarioParts = dayScenarios.ToList();
    }

    public WorkingScenario() { }
    public int Id { get; set; } = default;

    public List<DayScenario> ScenarioParts { get; set; } = new();
}