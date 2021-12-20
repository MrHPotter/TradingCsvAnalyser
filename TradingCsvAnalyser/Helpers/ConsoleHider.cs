using System;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace TradingCsvAnalyser.Helpers;

public class ConsoleHider
{
    [DllImport("kernel32.dll")]
    static extern IntPtr GetConsoleWindow();
    [DllImport("user32.dll")]
    static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    public static void HideConsole(){
        IntPtr h = GetConsoleWindow();
        ShowWindow(h, 0);
        
    }
}