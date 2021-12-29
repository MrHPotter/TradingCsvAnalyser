using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using TradingCsvAnalyser.Extensions.BaseTypeExtensions;

namespace TradingCsvAnalyser.Models.HelperModels;

public class AdditionalData
{
    public AdditionalData()
    {
        Content = new();
    }

    public AdditionalData(params object[] parameters)
    {
        Content = new();
        Content.AddRange(parameters);
    }
    public List<object> Content { get; set; }

    public override string ToString()
    {
        StringBuilder builder = new();
        foreach (var obj in Content)
        {
            builder.Append(nameof(obj));
            builder.Append(" : ");
            builder.Append(obj);
            builder.Append('|');
        }

        return builder.ToString();
    }

    public void Add(object obj)
    {
        Content.Add(obj);
    }

    public void AddNew(params object[] objects)
    {
        Content.AddNew(objects);
    }

    public void AddNew(object obj)
    {
        Content.AddNew(obj);
    }

    public void AddRange(params object[] objects)
    {
        Content.AddRange(objects);
    }
}