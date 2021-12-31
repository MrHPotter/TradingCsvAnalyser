using System.Collections.Generic;

namespace TradingCsvAnalyser.Extensions.BaseTypeExtensions;

public static class ObjectEnumerableExtensions
{
    public static void AddNew(this List<object> objects, object obj)
    {
        if (!objects.Contains(obj))
            objects.Add(obj);
    }

    public static void AddNew(this List<object> objects, params object[] parameters)
    {
        foreach (var obj in parameters)
        {
            objects.AddNew(obj);
        }
    }
}