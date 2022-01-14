using System.Text.Json;
using Common;
using Hw5.Exercise0.Models;

namespace Hw5.Exercise0.Core;

public static class JsonHandler
{
    public static IEnumerable<Currency> DeserealizeJson(IFileSystemProvider provider, string fileName)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        var stream = provider.Read(fileName);
        stream.Position = 0;
        var listCurrency = JsonSerializer.Deserialize<List<Currency>>(stream);
        return listCurrency is null ? new List<Currency>() : listCurrency;
    }

    public static bool IsValidDate(string date)
    {
        var todayDate = DateTime.Today.ToShortDateString();
        return date.Equals(todayDate, StringComparison.OrdinalIgnoreCase);
    }
}
