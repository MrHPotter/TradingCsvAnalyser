using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using TradingCsvAnalyser.Managers;
using TradingCsvAnalyser.Extensions;
using TradingCsvAnalyser.Models.Enums;
using TradingCsvAnalyser.Models.HelperModels;

namespace TradingCsvAnalyser.Windows;

public partial class ScenarioBuilderWindow : Window
{
    private readonly IChoiceManager _choiceManager;
    private FollowDayWindow _followDayWindow;
    private DayOfWeek? SelectedSourceDay => DayPicker.SelectedItem.GetDayOfWeek();
    private DayFilter? SelectedSourceDirection => DirectionPicker.SelectedItem.GetDayFilter();
    
    public ScenarioBuilderWindow(FollowDayWindow followDayWindow, IChoiceManager choiceManager)
    {
        _followDayWindow = followDayWindow;
        _choiceManager = choiceManager;
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
        _followDayWindow.Scenario.Clear();
    }

    private void ReverseButton_OnClick(object sender, RoutedEventArgs e)
    {
        _followDayWindow.Scenario.RemoveAt(_followDayWindow.Scenario.Count-1);
    }
}