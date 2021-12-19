using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls.Primitives;
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

        //Probably obsolete
        private void SubmitButtonTv_OnClick(object sender, RoutedEventArgs e)
        {
            SaveEntriesIfAny(ReadEntriesFromCsv<PriceEntry>());
        }

        private void SubmitButtonIv_OnClick(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(SymbolTextBox.Text)) return;
            
            SaveEntriesIfAny(ReadEntriesFromCsv<PriceDownload>()
                    .ToPriceEntries(SymbolTextBox.Text.ToUpper()));
        }

        private void SaveEntriesIfAny(IEnumerable<PriceEntry> currentPriceEntries)
        {
            currentPriceEntries = currentPriceEntries.ToList();
            if (currentPriceEntries.Any())
            {
                _data.PriceEntryRepository.AddNewEntries(currentPriceEntries);
                _data.SaveChanges();
            }
        }

        private void FileDialog_OnClick(object sender, RoutedEventArgs e)
        {
            
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() is true)
            {
                _filePath = dialog.FileName;
            }
        }

        private IEnumerable<T> ReadEntriesFromCsv<T>()
        {
            if (!String.IsNullOrWhiteSpace(_filePath) || _filePath.EndsWith(".csv"))
                throw new InvalidOperationException("Can only read Csv Files.");
            using var reader = new StreamReader(_filePath);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            return csv.GetRecords<T>().ToList();
        }

        public void Dispose()
        {
        }
    }
}