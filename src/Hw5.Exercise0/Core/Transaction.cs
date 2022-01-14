using System.Globalization;
using Hw5.Exercise0.Models;

namespace Hw5.Exercise0.Core;

public static class Transaction
{
    public static OperationData ParseArgs(string[] args)
    {
        var amount = -1m;
        if (decimal.TryParse(args[2], out var res))
        {
            amount = res;
        }
        return new OperationData(args[0], args[1], amount);
    }

    public static bool IsValidArgs(OperationData data, IEnumerable<Currency> listCurrency)
    {
        var result = GetMatchedCurrencies(listCurrency, data);

        if (data.OriginalCurrency.Equals("uah", StringComparison.OrdinalIgnoreCase))
        {
            result.Insert(0,
                new Currency("UAH",
                "Ukrainian hrivna",
                1m,
                DateTime.Today.ToString(DateTimeFormatInfo.InvariantInfo)));
        }
        if (data.DestinationCurrency.Equals("uah", StringComparison.OrdinalIgnoreCase))
        {
            result.Add(new Currency("UAH",
                "Ukrainian hrivna",
                1m,
                DateTime.Today.ToString(DateTimeFormatInfo.InvariantInfo)));
        }

        return result.Count == 2 && data.Amount != -1m;
    }

    public static ResultTransaction ProcessTransaction(IEnumerable<Currency> listCurrency, OperationData data)
    {
        var result = GetMatchedCurrencies(listCurrency, data);
        var firstRate = -1m;
        var secondRate = -1m;

        if (result.Count > 0)
        {
            firstRate = result[0].Rate;
            if (result.Count == 2)
            {
                secondRate = result[1].Rate;
            }
        }

        if (data.OriginalCurrency.Equals(data.DestinationCurrency, StringComparison.OrdinalIgnoreCase))
        {
            return new ResultTransaction(data.OriginalCurrency, data.Amount,
                DateTime.Today.ToString(DateTimeFormatInfo.InvariantInfo));
        }
        else if (data.DestinationCurrency.Equals("uah", StringComparison.OrdinalIgnoreCase))
        {
            return new ResultTransaction("uah",
                AmountToRatesUAH(firstRate, data.Amount),
                result.First().ExchangeDate);
        }
        else if (data.OriginalCurrency.Equals("uah", StringComparison.OrdinalIgnoreCase))
        {
            return new ResultTransaction(result.First().CurrencyCode,
                AmountToRatesUAH(firstRate, data.Amount),
                result.First().ExchangeDate);
        }
        else
        {
            return new ResultTransaction(result[1].CurrencyCode,
               AmountToRates(firstRate, data.Amount, secondRate),
               result.First().ExchangeDate);
        }
    }

    private static IList<Currency> GetMatchedCurrencies(IEnumerable<Currency> listCurrency, OperationData data)
    {
        return listCurrency.Where(x => x.CurrencyCode
                .Equals(data.OriginalCurrency, StringComparison.OrdinalIgnoreCase)
            || x.CurrencyCode
                .Equals(data.DestinationCurrency, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    private static decimal AmountToRatesUAH(decimal rate, decimal amount)
    {
        return rate * amount;
    }
    private static decimal AmountToRates(decimal rate, decimal amount, decimal secondRate)
    {
        return AmountToRatesUAH(secondRate, AmountToRatesUAH(rate, amount));
    }
}
