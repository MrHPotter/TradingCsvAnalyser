using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using TradingCsvAnalyser.DataProviders;
using TradingCsvAnalyser.Extensions;
using TradingCsvAnalyser.Models;

namespace TradingCsvAnalyser.Windows;

public partial class DetailAnalysisWindow : Window
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IUnitOfWork _data;
    
    private ObservableCollection<PriceEntryResult> _priceEntries;
    private string? _symbol() => SymbolPicker.SelectedItem?.ToString();
    private DayOfWeek? _dayOfWeek() => DayOfWeekPicker.SelectedItem.GetDayOfWeek();
    
    public DetailAnalysisWindow(IServiceProvider serviceProvider, IUnitOfWork data)
    {
        _serviceProvider = serviceProvider;
        _data = data;
        _priceEntries = new();
        InitializeComponent();
        SetupDayPicker();
        DetailGrid.ItemsSource = _priceEntries;
    }

    private void SetupDayPicker()
    {
        foreach (var dayOfWeek in Enum.GetValues<DayOfWeek>())
        {
            if (dayOfWeek.IsWeekDay())
                DayOfWeekPicker.Items.Add(dayOfWeek);
        }
    }

    private void GetterButton_OnClick(object sender, RoutedEventArgs e)
    {
        if(_symbol() is not null && _dayOfWeek() is not null)
        {
            _priceEntries.Clear();
            var newEntries = 
                _data.PriceEntryRepository.GetEntriesForDay(_symbol()!, _dayOfWeek()!.Value)
                    .OrderByDescending(pe => pe.DateAndTime).ToList();
            newEntries.ForEach(i => _priceEntries.Add(i));
        }
    }

    private void SymbolPicker_OnDropDownOpened(object? sender, EventArgs e)
    {
        var unit = _serviceProvider.GetRequiredService<IUnitOfWork>();
        var symbols = unit.PriceEntryRepository.GetAvailableSymbols();
        foreach (var symbol in symbols)
        {
            SymbolPicker.Items.AddNew(symbol);
        }
    }

    private void DetailGrid_OnAutoGeneratingColumn(object? sender, DataGridAutoGeneratingColumnEventArgs e)
    {
        if(e.PropertyType == typeof(decimal?) || e.PropertyType == typeof(decimal))
            (e.Column as DataGridTextColumn).Binding.StringFormat = "0.####";
    }
}