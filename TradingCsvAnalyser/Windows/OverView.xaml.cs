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
using TradingCsvAnalyser.Models;
using TradingCsvAnalyser.Models.AnalysisResults;
using TradingCsvAnalyser.Models.Enums;

namespace TradingCsvAnalyser.Windows;

public partial class OverView : Window
{
    private readonly IServiceProvider _serviceProvider;
    private string? _candleRangeSelection;
    private string? _selectedSymbol;
    private string? _selectedMethod;
    private readonly ObservableCollection<DoWOverview> _dayOfWeekData;
    public OverView(IServiceProvider serviceProvider)
    {
        _dayOfWeekData = new();
        
        _serviceProvider = serviceProvider;
        InitializeComponent();
        SetupMainDataGrid();
        SetupMethodList();
    }

    private void SetupMainDataGrid()
    {
        MainDataGrid.AutoGenerateColumns = true;
        MainDataGrid.ItemsSource = _dayOfWeekData;
        MainDataGrid.ItemTemplate = new DataTemplate(typeof(DayOfWeekData));
        MainDataGrid.UpdateLayout();
    }

    private void SetupMethodList()
    {
        foreach (var methodName in _serviceProvider.GetRequiredService<IAggregationManager>().GetDayOfWeekMethods())
        {
            if(methodName=="CallMethodByName")
                continue;
            MethodSelectorBox.Items.Add(methodName);
        }
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
        _candleRangeSelection = SelectorBox.SelectedItem?.ToString();
        Console.WriteLine($"Selected {_candleRangeSelection}");
    }

    private void SymbolSelectorBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        _selectedSymbol = SymbolSelectorBox.SelectedItem?.ToString();
        Console.WriteLine($"Selected {_selectedSymbol} as Symbol");
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
        if(Enum.TryParse(_candleRangeSelection,out CandleRange selection))
        {
            if(String.IsNullOrWhiteSpace(_selectedSymbol) || String.IsNullOrWhiteSpace(_selectedMethod))
                _dayOfWeekData.Add(new(aggregator.GetAverageRangePerDay(selection),"All",_candleRangeSelection));
            else
            {
                _dayOfWeekData.Add(new
                    (aggregator.CallMethodByName(_selectedMethod, selection, _selectedSymbol),_selectedSymbol,_candleRangeSelection));
            }
        }

    }

    private void MethodSelectorBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        _selectedMethod = MethodSelectorBox.SelectedItem.ToString();
    }
}