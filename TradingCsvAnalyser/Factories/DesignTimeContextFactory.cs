using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using TradingCsvAnalyser.Models.Database;

namespace TradingCsvAnalyser.Factories;

public class DesignTimeContextFactory : IDesignTimeDbContextFactory<AnalyserContext>
{
    public AnalyserContext CreateDbContext(string[] args)
    {
        return new AnalyserContext(new DbContextOptions<AnalyserContext>());
    }
}