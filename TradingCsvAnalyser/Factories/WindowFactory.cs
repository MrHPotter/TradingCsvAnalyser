using System;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;

namespace TradingCsvAnalyser.Factories;

public class WindowFactory : IWindowFactory
{
    private readonly IServiceProvider _provider;

    public WindowFactory(IServiceProvider provider)
    {
        _provider = provider;
    }

    public TWindow CreateWindow<TWindow>() where TWindow : Window
    {
        var window = _provider.GetRequiredService<TWindow>();
        window.Show();
        return window;
    }
}