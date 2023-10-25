using System.Net;

namespace ConsoleApp;

internal static class TestHttpClient
{
    internal static void Action()
    {
        CookieContainer cookies = new CookieContainer();
        HttpClientHandler handler = new HttpClientHandler
        {
            CookieContainer = cookies,
            UseCookies = true
        };

        HttpClient client = new HttpClient(handler);
        HttpResponseMessage response = client.GetAsync("http://napsc.nexdev.net/Auth").Result;

        Uri uri = new Uri("http://napsc.nexdev.net");
        IEnumerable<Cookie> responseCookies = cookies.GetCookies(uri).Cast<Cookie>();
        foreach (Cookie cookie in responseCookies)
            Console.WriteLine(cookie.Name + ": " + cookie.Value);

        Console.ReadLine();
    }
}

