using System;
using System.Windows;
using TradingCsvAnalyser.Managers;
using TradingCsvAnalyser.Extensions;

namespace TradingCsvAnalyser.Windows;

public partial class FollowDayWindow : Window
{
    private IServiceProvider _serviceProvider;
    private readonly IChoiceManager _choiceManager;
    public FollowDayWindow(IServiceProvider serviceProvider, IChoiceManager choiceManager)
    {
        _serviceProvider = serviceProvider;
        _choiceManager = choiceManager;
        InitializeComponent();
        FillSymbolSelector();
    }
    private void FillSymbolSelector()
    {
       SymbolPicker.Items.AddNew(_choiceManager.GetAvailableSymbols());
    }

    private void FillDayPickers()
    {
        var days = _choiceManager.GetValidDays();
        SourceDayPicker.Items.AddNew(days);
        FollowDayPicker.Items.AddNew(days);
    }
}