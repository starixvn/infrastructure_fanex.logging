using System.Net;

namespace ConsoleApp;

internal static class TestHttpClient
{
    internal static void Action()
    {
        var cookies = new CookieContainer();
        var handler = new HttpClientHandler
        {
            CookieContainer = cookies,
            UseCookies = true
        };

        var client = new HttpClient(handler);
        _ = client.GetAsync("http://napsc.nexdev.net/Auth").Result;

        var uri = new Uri("http://napsc.nexdev.net");
        var responseCookies = cookies.GetCookies(uri).Cast<Cookie>();
        foreach (var cookie in responseCookies)
        {
            Console.WriteLine(cookie.Name + ": " + cookie.Value);
        }

        Console.ReadLine();
    }
}

