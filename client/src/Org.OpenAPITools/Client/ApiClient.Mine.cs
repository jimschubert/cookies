using System;
using System.Linq;
using RestSharp;

namespace Org.OpenAPITools.Client
{
    public partial class ApiClient
    {
        private static String CookieKey = "session-id";
        private String cookie = null;
        partial void InterceptRequest(IRestRequest request)
        {
            if (cookie == null) return;
            Console.WriteLine("Add cookie to request: " + cookie);
            request.AddCookie(CookieKey, cookie);
        }

        partial void InterceptResponse(IRestRequest request, IRestResponse response)
        {
            var setCookie = response.Cookies.First(c => c.Name == CookieKey);
            if (setCookie != null)
            {
                cookie = setCookie.Value;
                Console.WriteLine("Cookie on response: " + cookie);
            }
        }
    }
}