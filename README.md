# Cookie passing example

This is an example for https://github.com/OpenAPITools/openapi-generator/issues/187.

It demonstrates re-using a cookie in ASP.NET Core 2.0 API between calls. It's very naive; this generates a new Guid if the cookie doesn't exist, or passes the same cookie back to the client if it does exist.

## Server

You can start the server (`cd server/src/Org.OpenAPITools && dotnet run`), then curl without a cookie:

```
curl -v http://localhost:5000/api/pets
```
And with a cookie:

```
curl --cookie 'session-id=fc1467d9d76144c4beeb49646d7cc76a' -v http://localhost:5000/api/pets
```
to see how the server implementation behaves.

NOTE: Querying via http://localhost:5000/swagger/ or another tool like Postman most likely won't display the cookie header.

## Client

The client doesn't persist cookies automatically across queries. This is up to a consumer to maintain.

This differs a bit from a browser-side framework like fetch or xhr, where you can set a "credentials" flag because those frameworks either ensure a single instance per action or
maintain a cookiejar in a single-threaded JavaScript environment.

C# is a little different. We have no control over whether or not an implementer has instantiated a client as a singleton object or if a Configuration instance is shared. In fact, the original 
C# client implementation of the template was written with both the Configuration and the ApiClient being singletons which weren't thread-safe. This has been mitigated a bit by creating better structured 
constructors on the Api generated types and on ApiClient. Lastly, we have no way of knowing if the client should parrot back cookies (and to which endpoints). We provide partial methods for implementing extensions
specifically for this reason. Users can intercept the request, response, and error handling.

I have a new template I'll be prposing to the community, likely for the OpenAPI Generator 4.x release. This new template will provide an abstraction 
around RESTful APIs similar to what RestSharp gives, but it decouples the generated template code from RestSharp. This allows consumers to implement other requestor libs, such as .NET's own HttpClient.

This sample demonstrates how to intercept the request and response to naively pass any existing cookie back to the server. Don't consider this thread-safe or even production-ready, it's merely an example of passing cookies presented by the service back to the service.

To do this, I've implemented a partial extension on ApiClient. (See `ApiClient.Mine.cs`):

```
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
```

Assuming you have the server running from the first section in this README, you can build and run the client to evaluate:

```
$ cd client
$ sh build.sh
$ csharp -r:bin/JsonSubTypes.dll -r:bin/Newtonsoft.Json.dll -r:bin/Org.OpenAPITools.dll -r:bin/RestSharp.dll
Mono C# Shell, type "help;" for help

Enter statements below.
csharp> using Org.OpenAPITools.Client;
csharp> using Org.OpenAPITools.Api;
csharp> using Org.OpenAPITools.Model;
csharp> Configuration.Default.BasePath = "http://localhost:5000/api";
csharp> var api = new DefaultApi();
csharp> api.PetsGet();
Cookie on response: 8ea9aa2dccb14801b7d43ff3d0e9013b
{ class Pet {
  Id: 1
  Name: Jim
  Tag: Person
}
, class Pet {
  Id: 2
  Name: Socks
  Tag: Cat
}
, class Pet {
  Id: 3
  Name: Fido
  Tag: Dog
}
, class Pet {
  Id: 4
  Name: Pookie
  Tag: Snake
}
 }
csharp> api.PetsGet();
Add cookie to request: 8ea9aa2dccb14801b7d43ff3d0e9013b
Cookie on response: 8ea9aa2dccb14801b7d43ff3d0e9013b
{ class Pet {
  Id: 1
  Name: Jim
  Tag: Person
}
, class Pet {
  Id: 2
  Name: Socks
  Tag: Cat
}
, class Pet {
  Id: 3
  Name: Fido
  Tag: Dog
}
, class Pet {
  Id: 4
  Name: Pookie
  Tag: Snake
}
 }
```
