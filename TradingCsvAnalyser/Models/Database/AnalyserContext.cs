using System.Windows;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;

namespace TradingCsvAnalyser.Models.Database;

public class AnalyserContext : DbContext
{
    public DbSet<PriceEntry> PriceEntries => Set<PriceEntry>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PriceEntry>().HasKey(entry => new { entry.Symbol,entry.DateAndTime });
        
        base.OnModelCreating(modelBuilder);
    }
}