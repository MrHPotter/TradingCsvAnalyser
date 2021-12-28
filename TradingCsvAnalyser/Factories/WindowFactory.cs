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
        return _provider.GetRequiredService<TWindow>();
    }
}