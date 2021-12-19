using System.Text.Json.Nodes;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;

namespace TradingCsvAnalyser.Models.Database;

public class AnalyserContext : DbContext
{
private const string ConnectionString = "Data Source=analyser.db";

public AnalyserContext(DbContextOptions<AnalyserContext> options) : base(options)
    {
        
    }
    public DbSet<PriceEntry> PriceEntries => Set<PriceEntry>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PriceEntry>().HasKey(entry => new { entry.Symbol,entry.DateAndTime });
        
        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite(ConnectionString);
        optionsBuilder.UseLazyLoadingProxies();
        
        base.OnConfiguring(optionsBuilder);
    }
}