using System;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;

namespace TradingCsvAnalyser.Windows;

public partial class OverView : Window
{
    private readonly IServiceProvider _serviceProvider;

    public OverView(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
    }

    public void OpenImportWindow(object sender, RoutedEventArgs e)
    {
        ImportWindow importWindow = _serviceProvider.GetRequiredService<ImportWindow>();
        importWindow.ShowDialog();
    }
}