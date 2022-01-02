using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using CsvHelper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;
using TradingCsvAnalyser.Appilication;
using TradingCsvAnalyser.DataProviders;
using TradingCsvAnalyser.Extensions;
using TradingCsvAnalyser.Factories;
using TradingCsvAnalyser.Managers;
using TradingCsvAnalyser.Models;
using TradingCsvAnalyser.Models.AnalysisResults;
using TradingCsvAnalyser.Models.AnalysisResults.DayOfWeekAnalysis;
using TradingCsvAnalyser.Models.Enums;
using TradingCsvAnalyser.Models.HelperModels;
using TradingCsvAnalyser.Providers;

namespace TradingCsvAnalyser.Windows;

public partial class OverView : Window
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IWindowFactory _windowFactory;
    private readonly ICsvProvider _csvProvider;
    private readonly IUnitOfWork _data;
    private IAnalyserConfig _config;
    private string? _candleRangeSelection;
    private string? _selectedSymbol;
    private string? _selectedMethod;
    private DayFilter _selectedDayFilter = DayFilter.None;
    private readonly DateRange _selectedDateRange;
    
    private readonly ObservableCollection<DoWOverviewStrings> _dayOfWeekData;
    
    public OverView(IServiceProvider serviceProvider, IWindowFactory windowFactory, ICsvProvider csvProvider, IAnalyserConfig config, IUnitOfWork data)
    {
        _selectedDateRange = new();
        _dayOfWeekData = new();
        _serviceProvider = serviceProvider;
        _windowFactory = windowFactory;
        _csvProvider = csvProvider;
        _config = config;
        _data = data;
        InitializeComponent();
        SetupMainDataGrid();
        FillSymbolSelector();
        SetupMethodList();
        SetupDayFilterBox();
        LoadStoredSession();
    }

    private void SetupMainDataGrid()
    {
        MainDataGrid.AutoGenerateColumns = true;
        MainDataGrid.ItemsSource = _dayOfWeekData;
        MainDataGrid.UpdateLayout();
    }

    private void LoadStoredSession()
    {
        
        foreach (var overview in _data.SessionRepository.GetSession<DoWOverviewStrings>())
        {
            _dayOfWeekData.Add(overview);
        }
    }

    private void LoadResults(string path)
    {
        foreach (var entry in _csvProvider.ReadEntriesFromCsv<DoWOverviewStrings>(path))
        {
            _dayOfWeekData.Add(entry);    
        }
    }
    private void StoreSessionResults()
    {
       _data.SessionRepository.SaveSession(_dayOfWeekData);
    }
    private void SetupMethodList()
    {
        foreach (var methodName in _serviceProvider.GetRequiredService<IDayOfWeekDataManager>().GetMethodNames())
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
        if(!DayRangeSelectorBox.HasItems)
            foreach (var value in Enum.GetValues(typeof(CandleRange)))
            {
                if (value is not null && !DayRangeSelectorBox.Items.Contains(value))
                    DayRangeSelectorBox.Items.Add(value);
            }
    }

    private void SelectorBox_OnSelectionChanged(object sender, RoutedEventArgs e)
    {
        _candleRangeSelection = DayRangeSelectorBox.SelectedItem?.ToString();
        Console.WriteLine($"Selected {_candleRangeSelection}");
    }

    private void SymbolSelectorBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        _selectedSymbol = SymbolSelectorBox.SelectedItem?.ToString();
        Console.WriteLine($"Selected {_selectedSymbol} as Symbol");
    }

    private void SymbolSelectorBox_OnDropDownOpened(object? sender, EventArgs e)
    {
        FillSymbolSelector();
    }

    private void FillSymbolSelector()
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
            {
                DoWOverview data = new
                    (aggregator.CallMethodByName(_selectedMethod, new DoWDefaultParameters
                            (candleRange, _selectedSymbol, _selectedDayFilter, _selectedDateRange))
                        ,_selectedSymbol,_candleRangeSelection, _selectedMethod, _selectedDayFilter, _selectedDateRange);
                _dayOfWeekData.Add(data);
            }
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

    private void DetailAnalysisButton_OnClick(object sender, RoutedEventArgs e)
    {
        _windowFactory.CreateWindow<DetailAnalysisWindow>();
    }

    private void EndDatePicker_OnSelectedDateChanged(object? sender, SelectionChangedEventArgs e)
    {
        _selectedDateRange.End = EndDatePicker.SelectedDate;
    }

    private void StartDatePicker_OnSelectedDateChanged(object? sender, SelectionChangedEventArgs e)
    {
        _selectedDateRange.Start = StartDatePicker.SelectedDate;
    }

    private void MainDataGrid_OnAutoGeneratingColumn(object? sender, DataGridAutoGeneratingColumnEventArgs e)
    {
        if(e.PropertyType == typeof(decimal?))
            (e.Column as DataGridTextColumn).Binding.StringFormat = "0.######";
    }

    private void FollowDayButton_OnClick(object sender, RoutedEventArgs e)
    {
        _windowFactory.CreateWindow<FollowDayWindow>();
    }

    private void SaveResultsButton_OnClick(object sender, RoutedEventArgs e)
    {
        var dialog = _csvProvider.CsvSaveDialog();
        if(dialog.ShowDialog(this) ?? false)
            _csvProvider.WriteEntriesToCsv(dialog.FileName, _dayOfWeekData);
    }
    

    private void LoadResultsButton_OnClick(object sender, RoutedEventArgs e)
    {
        var dialog = _csvProvider.CsvOpenDialog();
        if(dialog.ShowDialog() ?? false)
            LoadResults(dialog.FileName);
    }

    private void OverView_OnClosing(object? sender, CancelEventArgs e)
    {
        StoreSessionResults();
    }

    private void ClearingButton_OnClick(object sender, RoutedEventArgs e)
    {
        _dayOfWeekData.Clear();
    }

    private void DeleteButton_OnClick(object sender, RoutedEventArgs e)
    {
        _dayOfWeekData.RemoveAt(_dayOfWeekData.Count-1);
    }
}