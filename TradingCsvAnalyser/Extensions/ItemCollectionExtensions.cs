using System.Collections.Generic;
using System.Windows.Controls;

namespace TradingCsvAnalyser.Extensions;

public static class ItemCollectionExtensions
{
    public static void AddNew(this ItemCollection collection, object newItem)
    {
        if (!collection.Contains(newItem))
            collection.Add(newItem);
    }
    
    public static void AddNew(this ItemCollection collection, IEnumerable<object> newItems)
    {
        foreach (var newItem in newItems)
        {
            collection.AddNew(newItem);
        }
    }
}