using Common;
using Hw5.Exercise0.Core;
using RichardSzalay.MockHttp;

namespace Hw5.Exercise0;

public class HttpClientApplication
{
    private readonly IFileSystemProvider _fileSystemProvider;
    private readonly HttpClient _httpClient;
    private const string Cache = "cache.json";
    private const string RequestURL = "https://bank.gov.ua/NBUStatService/v1/statdirectory/exchange?json";

    public HttpClientApplication(MockHttpMessageHandler httpMessageHandler, IFileSystemProvider fileSystemProvider)
    {
        _fileSystemProvider = fileSystemProvider;
        _httpClient = httpMessageHandler.ToHttpClient();
    }

    /// <summary>
    /// Runs http client app.
    /// </summary>
    /// <param name="args">Command line arguments</param>
    /// <returns>
    /// Returns <see cref="ReturnCode.Success"/> in case of successful exchange calculation.
    /// Returns <see cref="ReturnCode.InvalidArgs"/> in case of invalid <paramref name="args"/>.
    /// Returns <see cref="ReturnCode.Error"/> in case of error <paramref name="args"/>.
    /// </returns>
    public ReturnCode Run(params string[] args)
    {
        if (args is null || args.Length != 3)
        {
            return ReturnCode.InvalidArgs;
        }

        if (!ArgumentsHandler.IsValidArgs(args))
        {
            return ReturnCode.InvalidArgs;
        }

        if (!_fileSystemProvider.Exists(Cache))
        {
            if (!RequestHandler.TryRequestAndWrite(_httpClient, RequestURL, Cache, _fileSystemProvider))
            {
                return ReturnCode.Error;
            }
            return ReturnCode.Success;
        }

        var listCurrency = JsonHandler.DeserealizeJson(_fileSystemProvider, Cache);
        if (listCurrency is null || !listCurrency.Any())
        {
            return ReturnCode.Error;
        }

        if (!JsonHandler.IsValidDate(listCurrency.First().ExchangeDate))
        {
            if (!RequestHandler.TryRequestAndWrite(_httpClient, RequestURL, Cache, _fileSystemProvider))
            {
                return ReturnCode.Error;
            }

            listCurrency = JsonHandler.DeserealizeJson(_fileSystemProvider, Cache);
            if (listCurrency is null || !listCurrency.Any())
            {
                return ReturnCode.Error;
            }
        }

        var data = Transaction.ParseArgs(args);

        if (!Transaction.IsValidArgs(data, listCurrency))
        {
            return ReturnCode.InvalidArgs;
        }

        var result = Transaction.ProcessTransaction(listCurrency, data);

        Console.Write($"{result.Currency} {result.Date} {result.Amount}");

        return ReturnCode.Success;
    }
}
