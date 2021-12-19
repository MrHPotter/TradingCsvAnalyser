using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using CsvHelper;
using Microsoft.Win32;
using TradingCsvAnalyser.DataProviders;
using TradingCsvAnalyser.Models;

namespace TradingCsvAnalyser.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ImportWindow : Window, IDisposable
    {
        private IEnumerable<PriceEntry>? _currentPriceEntries;
        private readonly IUnitOfWork _data;
        public ImportWindow(IUnitOfWork data)
        {
            _data = data;
            InitializeComponent();
        }

        private void SubmitButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (_currentPriceEntries?.Any() ?? false)
            {
                _data.PriceEntryRepository.AddNewEntries(_currentPriceEntries);
                //Import to Database
            }
        }
        
        private void FileDialog_OnClick(object sender, RoutedEventArgs e)
        {
            
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() is true)
            {
                var path = dialog.FileName;
                if (!path.EndsWith(".csv"))
                    throw new InvalidOperationException("Can only read Csv Files.");
                using (var reader = new StreamReader(path))
                {
                    using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                    {
                        _currentPriceEntries = csv.GetRecords<PriceEntry>();
                    }
                }
                
            }
        }

        public void Dispose()
        {
        }
    }
}