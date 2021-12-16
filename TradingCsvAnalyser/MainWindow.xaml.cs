using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CsvHelper;
using Microsoft.Win32;
using TradingCsvAnalyser.Models;

namespace TradingCsvAnalyser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IEnumerable<PriceEntry>? _currentPriceEntries;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void SubmitButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (_currentPriceEntries?.Any() ?? false)
            {
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
    }
}