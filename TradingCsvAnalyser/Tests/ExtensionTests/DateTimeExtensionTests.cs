using System;
using FluentAssertions;
using NUnit.Framework;
using TradingCsvAnalyser.Extensions;

namespace TradingCsvAnalyser.Tests.ExtensionTests;

[TestFixture]
public class DateTimeExtensionTests
{
    [TestCase(2021,5,20,20)]
    [TestCase(2021,7,20,29)]
    [TestCase(2021,10,24,42)]
    public void GetWeekOfYear_ReturnsCorrectWeeks(int year, int month, int day, int expectedResult)
    {
        var toTest = new DateTime(year, month, day);
        toTest.GetWeekOfYear().Should().Be(expectedResult);
    }
}