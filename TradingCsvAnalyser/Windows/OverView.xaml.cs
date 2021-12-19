using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using TradingCsvAnalyser.DataProviders;
using TradingCsvAnalyser.Extensions;
using TradingCsvAnalyser.Models.Enums;

namespace TradingCsvAnalyser.Windows;

public partial class OverView : Window
{
    private readonly IServiceProvider _serviceProvider;
    private string? CandleRangeSelection;
    private string? SelectedSymbol;
    public OverView(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
    }

    public void OpenImportWindow(object sender, RoutedEventArgs e)
    {
        ImportWindow importWindow = _serviceProvider.GetRequiredService<ImportWindow>();
        importWindow.ShowDialog();
    }
    
    private void SelectorBox_OnDropDownOpened(object? sender, EventArgs e)
    {
        Console.WriteLine("Opening Drop Down Menu");
        if(!SelectorBox.HasItems)
            foreach (var value in Enum.GetValues(typeof(CandleRange)))
            {
                if (value is not null && !SelectorBox.Items.Contains(value))
                    SelectorBox.Items.Add(value);
            }
    }

    private void SelectorBox_OnSelectionChanged(object sender, RoutedEventArgs e)
    {
        CandleRangeSelection = SelectorBox.SelectedItem?.ToString();
        Console.WriteLine($"Selected {CandleRangeSelection}");
    }

    private void SymbolSelectorBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        SelectedSymbol = SymbolSelectorBox.SelectedItem?.ToString();
        Console.WriteLine($"Selected {SelectedSymbol} as Symbol");
    }

    private void SymbolSelectorBox_OnDropDownOpened(object? sender, EventArgs e)
    {
        var unit = _serviceProvider.GetRequiredService<IUnitOfWork>();
        var symbols = unit.PriceEntryRepository.GetAvailableSymbols();
        foreach (var symbol in symbols)
        {
            SymbolSelectorBox.Items.AddNew(symbol);
        }
    }
    
}