using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Win32;

namespace TradingCsvAnalyser.Providers;

public class CsvProvider : ICsvProvider
{
    public IEnumerable<T> ReadEntriesFromCsv<T>(string filePath)
    {
        try
        {
            EnsureCsvSelected(filePath);
            FormatTradingViewCsv(filePath);
            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            
            return csv.GetRecords<T>().ToList();
        }
        catch (Exception e)
        {
            throw new WarningException("Bad CSV Format", e);
        }
    }

    public void WriteEntriesToCsv<T>(string filePath, IEnumerable<T> data)
    {
        try
        {
            var stream = File.OpenWrite(filePath);
            using var writer = new StreamWriter(stream);
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
            csv.WriteRecords(data);
        }
        catch (CsvHelperException e)
        {
            throw new WarningException("Bad CSV Format", e);
        }
        
    }

    public SaveFileDialog CsvSaveDialog()
    {
        var dialog = new SaveFileDialog();
        dialog.FileName = "AnalyserWorkspace";
        dialog.InitialDirectory = AppContext.BaseDirectory + "Export";
        dialog.DefaultExt = ".csv";
        dialog.Filter = "Csv files (.csv) | *.csv";
        return dialog;
    }

    public OpenFileDialog CsvOpenDialog()
    {
        var dialog = new OpenFileDialog();
        dialog.InitialDirectory = AppContext.BaseDirectory + "Csv\\Export";
        dialog.DefaultExt = ".csv";
        dialog.Filter = "Csv files (.csv) | *.csv";
        return dialog;
    }

    private void FormatTradingViewCsv(string filePath)
    {
        var file = File.ReadAllText(filePath);
        file = file.Replace("time", "Date", StringComparison.CurrentCultureIgnoreCase);
        file = file.Replace("open", "Open", StringComparison.CurrentCultureIgnoreCase);
        file = file.Replace("high", "High", StringComparison.CurrentCultureIgnoreCase);
        file = file.Replace("low", "Low", StringComparison.CurrentCultureIgnoreCase);
        file = file.Replace("close", "Price", StringComparison.CurrentCultureIgnoreCase);
        File.WriteAllText(filePath,file);
    }

    private void EnsureCsvSelected(string filePath)
    {
        if (String.IsNullOrWhiteSpace(filePath) || !filePath.EndsWith(".csv"))
            throw new InvalidOperationException("Can only read Csv Files.");
    }

}