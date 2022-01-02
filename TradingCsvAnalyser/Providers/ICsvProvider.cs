using System.Collections.Generic;
using Microsoft.Win32;

namespace TradingCsvAnalyser.Providers;

public interface ICsvProvider
{
    public IEnumerable<T> ReadEntriesFromCsv<T>(string filePath);

    public void WriteEntriesToCsv<T>(string filePath, IEnumerable<T> data);

    public OpenFileDialog CsvOpenDialog();

    public SaveFileDialog CsvSaveDialog();
}