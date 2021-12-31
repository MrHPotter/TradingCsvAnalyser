using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using TradingCsvAnalyser.Managers;
using TradingCsvAnalyser.Extensions;
using TradingCsvAnalyser.Models.AnalysisResults;
using TradingCsvAnalyser.Models.Enums;
using TradingCsvAnalyser.Models.HelperModels;
using TradingCsvAnalyser.Models.HelperModels.FollowDayAnalysis;

namespace TradingCsvAnalyser.Windows;

public partial class FollowDayWindow : Window
{
    private IServiceProvider _serviceProvider;
    private readonly IChoiceManager _choiceManager;
    private readonly IFollowDayManager _followDayManager;
    private string? SelectedSymbol => SymbolPicker.SelectedItem?.ToString();
    private DayOfWeek? SelectedSourceDay => SourceDayPicker.SelectedItem.GetDayOfWeek();
    private DayFilter? SelectedSourceDirection => SourceDirectionPicker.SelectedItem.GetDayFilter();
    private DayOfWeek? SelectedFollowDay => FollowDayPicker.SelectedItem.GetDayOfWeek();
    private DateTime? SelectedStartDate => StartDatePicker.SelectedDate;
    private DateTime? SelectedEndDate => EndDatePicker.SelectedDate;
    
    private List<Window> Children { get; set; } 
    
    public ObservableCollection<DayScenario> Scenario { get; set; } 

    private readonly ObservableCollection<FollowDayReport> _followDayReports;

    public FollowDayWindow(IServiceProvider serviceProvider, IChoiceManager choiceManager, IFollowDayManager followDayManager)
    {
        Children = new();
        Scenario = new();
        _followDayReports = new();
        
        _serviceProvider = serviceProvider;
        _choiceManager = choiceManager;
        _followDayManager = followDayManager;
        InitializeComponent();
        FillSymbolSelector();
        FillDayPickers();
        FillDirectionPicker();
        FollowResultsGrid.ItemsSource = _followDayReports;
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
        var builder = new ScenarioBuilderWindow(this, _choiceManager);
        builder.Show();
        Children.Add(builder);
    }

    private void FollowDayWindow_OnClosing(object? sender, CancelEventArgs e)
    {
        foreach (var window in Children)
        {
            if(window.IsVisible)
                window.Close();
        }
    }
}