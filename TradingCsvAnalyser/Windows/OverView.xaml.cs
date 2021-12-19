using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using Microsoft.Extensions.DependencyInjection;
using TradingCsvAnalyser.DataProviders;
using TradingCsvAnalyser.Extensions;
using TradingCsvAnalyser.Managers;
using TradingCsvAnalyser.Models.AnalysisResults;
using TradingCsvAnalyser.Models.Enums;

namespace TradingCsvAnalyser.Windows;

public partial class OverView : Window
{
    private readonly IServiceProvider _serviceProvider;
    private string? CandleRangeSelection;
    private string? SelectedSymbol;
    private readonly ObservableCollection<DayOfWeekData> _dayOfWeekData;
    public OverView(IServiceProvider serviceProvider)
    {
        _dayOfWeekData = new();
        
        _serviceProvider = serviceProvider;
        InitializeComponent();
        var template = new DataTemplate(typeof(DayOfWeekData));
        MainDataGrid.AutoGenerateColumns = true;
        MainDataGrid.ItemsSource = _dayOfWeekData;
        MainDataGrid.ItemTemplate = new DataTemplate(typeof(DayOfWeekData));
        MainDataGrid.UpdateLayout();
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

    private void AnalyseButton_OnClick(object sender, RoutedEventArgs e)
    {
        var aggregator = _serviceProvider.GetRequiredService<IAggregationManager>();
        if(Enum.TryParse(CandleRangeSelection,out CandleRange selection))
            _dayOfWeekData.Add(aggregator.GetAverageRangePerDay(selection));

    }
}