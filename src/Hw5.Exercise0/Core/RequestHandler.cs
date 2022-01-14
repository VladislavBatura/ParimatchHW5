using Common;

namespace Hw5.Exercise0.Core;

public static class RequestHandler
{
    public static bool TryRequestAndWrite(HttpClient httpClient,
        string requestURL, string fileName,
        IFileSystemProvider provider)
    {
        var result = httpClient.GetAsync(requestURL).Result;
        if (!result.IsSuccessStatusCode)
            return false;
        var stream = result.Content.ReadAsStreamAsync().Result;
        stream.Position = 0;
        provider.Write(fileName, stream);
        return true;
    }
}
