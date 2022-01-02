using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using TradingCsvAnalyser.Managers;
using TradingCsvAnalyser.Extensions;
using TradingCsvAnalyser.Models.Enums;
using TradingCsvAnalyser.Models.HelperModels;
using TradingCsvAnalyser.Models.HelperModels.FollowDayAnalysis;
using TradingCsvAnalyser.Providers;

namespace TradingCsvAnalyser.Windows;

public partial class ScenarioBuilderWindow : Window
{
    private readonly IChoiceManager _choiceManager;
    private readonly ICsvProvider _csvProvider;
    private FollowDayWindow _followDayWindow;
    private DayOfWeek? SelectedSourceDay => DayPicker.SelectedItem.GetDayOfWeek();
    private DayFilter? SelectedSourceDirection => DirectionPicker.SelectedItem.GetDayFilter();
    
    public ScenarioBuilderWindow(FollowDayWindow followDayWindow, IChoiceManager choiceManager, ICsvProvider csvProvider)
    {
        _followDayWindow = followDayWindow;
        _choiceManager = choiceManager;
        _csvProvider = csvProvider;
        InitializeComponent();
        FillDayPickers();
        FillDirectionPicker();
        ScenarioGrid.ItemsSource = _followDayWindow.Scenario;
    }
    
    private void FillDayPickers()
    {
        DayPicker.Items.AddNew(_choiceManager.GetValidDays().Select(i => i.ToString()));
    }

    private void FillDirectionPicker()
    {
        DirectionPicker.Items.AddNew(_choiceManager.GetValidDirections().Select(i => i.ToString()));
    }
    private void AdderButton_OnClick(object sender, RoutedEventArgs e)
    {
        if (SelectedSourceDay is null)
            throw new ArgumentException("Please Select a Day to Add to your Scenario");
        if (SelectedSourceDirection is null)
            throw new ArgumentException("Please select a Direction to add to your Scenario");
        if (_followDayWindow.Scenario.FirstOrDefault(i => i.DayOfWeek ==SelectedSourceDay.Value) is null)
        {
            _followDayWindow.Scenario.Add(new DayScenario(SelectedSourceDay.Value, SelectedSourceDirection.Value));
        }
        
    }

    private void ClearerButton_OnClick(object sender, RoutedEventArgs e)
    {
        ClearScenario();
    }

    private void ClearScenario()
    {
        _followDayWindow.Scenario.Clear();
    }

    private void ReverseButton_OnClick(object sender, RoutedEventArgs e)
    {
        _followDayWindow.Scenario.RemoveAt(_followDayWindow.Scenario.Count-1);
    }

    private void ExportButton_OnClick(object sender, RoutedEventArgs e)
    {
        var dialog = _csvProvider.CsvSaveDialog();
        if(dialog.ShowDialog(this)?? false)
            _csvProvider.WriteEntriesToCsv(dialog.FileName,_followDayWindow.Scenario);
    }

    private void ImportButton_OnClick(object sender, RoutedEventArgs e)
    {
        var dialog = _csvProvider.CsvOpenDialog();
        if (dialog.ShowDialog(this) ?? false)
        {
            ClearScenario();
            foreach (var scenario in _csvProvider.ReadEntriesFromCsv<DayScenario>(dialog.FileName))
            {
                _followDayWindow.Scenario.Add(scenario);
            }
        }
    }

    private void ScenarioGrid_OnAutoGeneratingColumn(object? sender, DataGridAutoGeneratingColumnEventArgs e)
    {
        PropertyDescriptor propertyDescriptor = (PropertyDescriptor)e.PropertyDescriptor;
        if (propertyDescriptor.DisplayName == "WorkingScenarios")
        {
            e.Cancel = true;
        }
    }
}