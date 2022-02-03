using Microsoft.EntityFrameworkCore;
using TradingCsvAnalyser.Models.AnalysisResults.DayOfWeekAnalysis;
using TradingCsvAnalyser.Models.AnalysisResults.FollowDayReport;
using TradingCsvAnalyser.Models.HelperModels;
using TradingCsvAnalyser.Models.HelperModels.FollowDayAnalysis;

namespace TradingCsvAnalyser.Models.Database;

public class SessionContext : DbContext
{
    public SessionContext(DbContextOptions<SessionContext> options) : base(options)
    {
        
    }
    public DbSet<DoWOverviewStrings> DoWOverviewReports => Set<DoWOverviewStrings>();
    public DbSet<FollowDayReportStrings> FollowDayReports => Set<FollowDayReportStrings>();
    public DbSet<DayScenario> DayScenarios => Set<DayScenario>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DoWOverviewStrings>().HasKey(i => i.Id).HasName("Id");
        modelBuilder.Entity<FollowDayReportStrings>().HasKey(i => i.Id).HasName("Id");
        modelBuilder.Entity<DayScenario>().HasKey(i => new {i.Direction, i.DayOfWeek});
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=session.db");

        base.OnConfiguring(optionsBuilder);
    }
}