using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using Castle.Core.Internal;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Win32;
using TradingCsvAnalyser.DataProviders;
using TradingCsvAnalyser.Extensions.DataModels;
using TradingCsvAnalyser.Models;
using TradingCsvAnalyser.Models.SourceModels;

namespace TradingCsvAnalyser.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ImportWindow : Window, IDisposable
    {
        private string _filePath ="";
        private readonly IUnitOfWork _data;
        public ImportWindow(IUnitOfWork data)
        {
            _data = data;
            InitializeComponent();
        }

        private void SubmitButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(SymbolTextBox.Text)) return;
            
            SaveEntriesIfAny(ReadEntriesFromCsv<PriceDownload>()
                    .ToPriceEntries(SymbolTextBox.Text.ToUpper()));
            StatusLabel.Foreground = Brushes.Green;
            StatusLabel.Content = "CSV import Successful";
        }

        private void SaveEntriesIfAny(IEnumerable<PriceEntry> currentPriceEntries)
        {
            currentPriceEntries = currentPriceEntries.ToList();
            if (currentPriceEntries.Any())
            {
                _data.PriceEntryRepository.AddNewEntries(currentPriceEntries);
                _data.SaveAnalyserChanges();
            }
        }

        private void FileDialog_OnClick(object sender, RoutedEventArgs e)
        {
            StatusLabel.Content = "";
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() is true)
            {
                _filePath = dialog.FileName;
            }
        }

        private IEnumerable<T> ReadEntriesFromCsv<T>()
        {
            try
            {
                EnsureCsvSelected();
                FormatTradingViewCsv();
                using var reader = new StreamReader(_filePath);
                using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
                return csv.GetRecords<T>().ToList();
            }
            catch (Exception e)
            {
                StatusLabel.Content = "Bad CSV Format.Refer to Readme!";
                throw new WarningException("Bad CSV Format", e);
            }
        }

        private void FormatTradingViewCsv()
        {
            var file = File.ReadAllText(_filePath);
            file = file.Replace("time", "Date", StringComparison.CurrentCultureIgnoreCase);
            file = file.Replace("open", "Open", StringComparison.CurrentCultureIgnoreCase);
            file = file.Replace("high", "High", StringComparison.CurrentCultureIgnoreCase);
            file = file.Replace("low", "Low", StringComparison.CurrentCultureIgnoreCase);
            file = file.Replace("close", "Price", StringComparison.CurrentCultureIgnoreCase);
            File.WriteAllText(_filePath,file);
        }

        private void EnsureCsvSelected()
        {
            if (String.IsNullOrWhiteSpace(_filePath) || !_filePath.EndsWith(".csv"))
                throw new InvalidOperationException("Can only read Csv Files.");
        }

        public void Dispose()
        {
        }
    }
}