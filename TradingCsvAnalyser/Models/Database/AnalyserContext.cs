using System.Text.Json.Nodes;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;

namespace TradingCsvAnalyser.Models.Database;

public class AnalyserContext : DbContext
{
private const string ConnectionString = "SERVER=localhost; DATABASE=AnalyserTest; UID=root; PASSWORD=example";

public AnalyserContext(DbContextOptions<AnalyserContext> options) : base(options)
    {
        
    }
    public DbSet<PriceEntry> PriceEntries => Set<PriceEntry>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PriceEntry>().HasKey(entry => new { entry.Symbol,entry.DateAndTime });
        modelBuilder.Entity<PriceEntry>().HasIndex(p => p.Symbol).IsUnique(false);
        
        
        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySql(ConnectionString, ServerVersion.AutoDetect(ConnectionString));
        optionsBuilder.UseLazyLoadingProxies();
        
        base.OnConfiguring(optionsBuilder);
    }
}