using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Castle.Core.Internal;
using Microsoft.Win32;
using TradingCsvAnalyser.DataProviders;
using TradingCsvAnalyser.Managers;
using TradingCsvAnalyser.Extensions;
using TradingCsvAnalyser.Models.AnalysisResults;
using TradingCsvAnalyser.Models.AnalysisResults.FollowDayReport;
using TradingCsvAnalyser.Models.Enums;
using TradingCsvAnalyser.Models.HelperModels;
using TradingCsvAnalyser.Models.HelperModels.FollowDayAnalysis;
using TradingCsvAnalyser.Providers;

namespace TradingCsvAnalyser.Windows;

public partial class FollowDayWindow : Window
{
    private IServiceProvider _serviceProvider;
    private readonly IChoiceManager _choiceManager;
    private readonly IFollowDayManager _followDayManager;
    private readonly ICsvProvider _csvProvider;
    private readonly IUnitOfWork _data;
    private string? SelectedSymbol => SymbolPicker.SelectedItem?.ToString();
    private DayOfWeek? SelectedSourceDay => SourceDayPicker.SelectedItem.GetDayOfWeek();
    private DayFilter? SelectedSourceDirection => SourceDirectionPicker.SelectedItem.GetDayFilter();
    private DayOfWeek? SelectedFollowDay => FollowDayPicker.SelectedItem.GetDayOfWeek();
    private DateTime? SelectedStartDate => StartDatePicker.SelectedDate;
    private DateTime? SelectedEndDate => EndDatePicker.SelectedDate;
    
    private List<Window> Children { get; set; } 
    
    public ObservableCollection<DayScenario> Scenario { get; set; } 

    private readonly ObservableCollection<FollowDayReportStrings> _followDayReports;

    public FollowDayWindow(IServiceProvider serviceProvider, IChoiceManager choiceManager, IFollowDayManager followDayManager, ICsvProvider csvProvider, IUnitOfWork data)
    {
        Children = new();
        Scenario = new();
        _followDayReports = new();
        
        _serviceProvider = serviceProvider;
        _choiceManager = choiceManager;
        _followDayManager = followDayManager;
        _csvProvider = csvProvider;
        _data = data;
        InitializeComponent();
        FillSymbolSelector();
        FillDayPickers();
        FillDirectionPicker();
        FollowResultsGrid.ItemsSource = _followDayReports;
        ReloadSession();
    }
    private void FillSymbolSelector()
    {
       SymbolPicker.Items.AddNew(_choiceManager.GetAvailableSymbols());
    }

    private void FillDayPickers()
    {
        var days = _choiceManager.GetValidDays();
        SourceDayPicker.Items.AddNew(days.Select(i => i.ToString()));
        FollowDayPicker.Items.AddNew(days.Select(i => i.ToString()));
    }

    private void FillDirectionPicker()
    {
        foreach (var direction in _choiceManager.GetValidDirections())
        {
            SourceDirectionPicker.Items.AddNew(direction.ToString());
        }
    }
    private void AnalyserButton_OnClick(object sender, RoutedEventArgs e)
    {

        if (SelectedSymbol is not null && SelectedSourceDay is not null && SelectedSourceDirection is not null &&
            SelectedFollowDay is not null)
        {
            //Todo: Scenario bauen und einfügen
            
            var sourceScenario = new DayScenario(SelectedSourceDay.Value, SelectedSourceDirection.Value);
            var parameters = new FollowDayParameters(
                new DateRange(SelectedStartDate, SelectedEndDate), SelectedSymbol,sourceScenario, SelectedFollowDay.Value, Scenario);
            _followDayReports.Add(_followDayManager.GetFollowDayReport(parameters));
        }
        else
        {
            throw new ArgumentException("Please Select All Parameters - Dates not required");
        }
            
    }

    private void FollowResultsGrid_OnAutoGeneratingColumn(object? sender, DataGridAutoGeneratingColumnEventArgs e)
    {
        if(e.PropertyType == typeof(decimal?) || e.PropertyType == typeof(decimal))
            (e.Column as DataGridTextColumn).Binding.StringFormat = "0.######";
    }

    private void SymbolPicker_OnDropDownOpened(object? sender, EventArgs e)
    {
        SymbolPicker.Items.AddNew(_choiceManager.GetAvailableSymbols());
    }

    private void BuilderWindowButton_OnClick(object sender, RoutedEventArgs e)
    {
        var builder = new ScenarioBuilderWindow(this, _choiceManager, _csvProvider);
        builder.Show();
        Children.Add(builder);
    }

    private void FollowDayWindow_OnClosing(object? sender, CancelEventArgs e)
    {
        SaveSession();
        foreach (var window in Children)
        {
            if(window.IsVisible)
                window.Close();
        }
    }

    private void ClearingButton_OnClick(object sender, RoutedEventArgs e)
    {
        _followDayReports.Clear();
    }

    private void RevertButton_OnClick(object sender, RoutedEventArgs e)
    {
        _followDayReports.RemoveAt(_followDayReports.Count-1);
    }

    private void ExportButton_OnClick(object sender, RoutedEventArgs e)
    {
        var dialog = _csvProvider.CsvSaveDialog();
        if (dialog.ShowDialog(this) ?? false)
        {
            _csvProvider.WriteEntriesToCsv(dialog.FileName, _followDayReports);
            _csvProvider.WriteEntriesToCsv(dialog.FileName.Replace(".csv","Scenario.csv"),Scenario);
        }
    }
    
    //Todo eigene erbende Dialog Klasse

   

    private void ImportReportsButton_OnClick(object sender, RoutedEventArgs e)
    {
        var dialog = _csvProvider.CsvOpenDialog();
        if (dialog.ShowDialog(this) ?? false)
        {
            foreach (var entry in _csvProvider.ReadEntriesFromCsv<FollowDayReportStrings>(dialog.FileName))
            {
                _followDayReports.Add(entry);
            }
        }
    }

    private void ReloadSession()
    {
        foreach (var analysis in _data.SessionRepository.GetSession<FollowDayReportStrings>())
        {
            _followDayReports.Add(analysis);
        }
        
        foreach (var part in _data.SessionRepository.GetSession<DayScenario>())
        {
            Scenario.Add(part);
        }
        
        
    }
    private void SaveSession()
    {
        _data.SessionRepository.SaveSession(_followDayReports);
        _data.SessionRepository.SaveSession(Scenario);
    }
    
}