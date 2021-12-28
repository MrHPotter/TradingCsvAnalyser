using System.Windows;

namespace TradingCsvAnalyser.Factories;

public interface IWindowFactory
{
    public TWindow CreateWindow<TWindow>() where TWindow : Window;
}