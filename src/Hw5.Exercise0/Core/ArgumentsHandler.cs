using ISO._4217;

namespace Hw5.Exercise0.Core;

public static class ArgumentsHandler
{
    public static bool IsValidArgs(string[] args)
    {
        var returnedCurrency = CurrencyCodesResolver.GetCurrenciesByCode(args[0]);
        if (args[0].Length is < 3 or > 3)
        {
            return false;
        }
        else if (!returnedCurrency.Any() ||
                returnedCurrency.First().Code.Equals("xxx", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        returnedCurrency = CurrencyCodesResolver.GetCurrenciesByCode(args[1]);
        if (args[1].Length is < 3 or > 3)
        {
            return false;
        }
        else if (!returnedCurrency.Any() ||
                returnedCurrency.First().Code.Equals("xxx", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        if (!decimal.TryParse(args[2], out _))
        {
            return false;
        }

        return true;
    }
}
