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
using TradingCsvAnalyser.Models.HelperModels;

namespace TradingCsvAnalyser.Windows;

public partial class OverView : Window
{
    private readonly IServiceProvider _serviceProvider;
    private string? _candleRangeSelection;
    private string? _selectedSymbol;
    private string? _selectedMethod;
    private DayFilter _selectedDayFilter = DayFilter.None;
    private readonly ObservableCollection<DoWOverview> _dayOfWeekData;
    public OverView(IServiceProvider serviceProvider)
    {
        _dayOfWeekData = new();
        
        _serviceProvider = serviceProvider;
        InitializeComponent();
        SetupMainDataGrid();
        SetupMethodList();
        SetupDayFilterBox();
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
        foreach (var methodName in _serviceProvider.GetRequiredService<IDayOfWeekDataManager>().GetDayOfWeekMethods())
        {
            if(methodName=="CallMethodByName")
                continue;
            MethodSelectorBox.Items.Add(methodName);
        }
    }

    private void SetupDayFilterBox()
    {
        var values = Enum.GetValues<DayFilter>();
        foreach (var filter in values)
        {
            DayFilterBox.Items.Add(filter);
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
        var aggregator = _serviceProvider.GetRequiredService<IDayOfWeekDataManager>();
        if(Enum.TryParse(_candleRangeSelection,out CandleRange candleRange))
        {
            if(!String.IsNullOrWhiteSpace(_selectedSymbol) && !String.IsNullOrWhiteSpace(_selectedMethod))
                _dayOfWeekData.Add(new
                    (aggregator.CallMethodByName(_selectedMethod, new DoWDefaultParameters(candleRange, _selectedSymbol, _selectedDayFilter))
                        ,_selectedSymbol,_candleRangeSelection, _selectedMethod, _selectedDayFilter));
            else
            {
                throw new ArgumentException("Please Select a Symbol and Method");
            }
        }

    }

    private void MethodSelectorBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        _selectedMethod = MethodSelectorBox.SelectedItem.ToString();
    }

    private void DayFilterBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (Enum.TryParse<DayFilter>(DayFilterBox.SelectedItem.ToString(), out var result))
            _selectedDayFilter = result;
    }
}