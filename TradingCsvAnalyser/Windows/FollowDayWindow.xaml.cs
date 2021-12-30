using System;
using System.Collections.ObjectModel;
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
    private string? _selectedSymbol => SymbolPicker.SelectedItem?.ToString();
    private DayOfWeek? _selectedSourceDay => SourceDayPicker.SelectedItem.GetDayOfWeek();
    private DayFilter? _selectedSourceDirection => SourceDirectionPicker.SelectedItem.GetDayFilter();
    private DayOfWeek? _selectedFollowDay => FollowDayPicker.SelectedItem.GetDayOfWeek();
    private DateTime? _selectedStartDate => StartDatePicker.SelectedDate;
    private DateTime? _selectedEndDate => EndDatePicker.SelectedDate;

    private ObservableCollection<FollowDayReport> _followDayReports;

    public FollowDayWindow(IServiceProvider serviceProvider, IChoiceManager choiceManager, IFollowDayManager followDayManager)
    {
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

        if (_selectedSymbol is not null && _selectedSourceDay is not null && _selectedSourceDirection is not null &&
            _selectedFollowDay is not null)
        {
            var parameters = new FollowDayParameters(
                new DateRange(_selectedStartDate, _selectedEndDate), _selectedSymbol, _selectedSourceDay.Value,
                _selectedSourceDirection.Value, _selectedFollowDay.Value);
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
}