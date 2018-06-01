# Org.OpenAPITools.Api.DefaultApi

All URIs are relative to *http://petstore.swagger.io/api*

Method | HTTP request | Description
------------- | ------------- | -------------
[**PetsGet**](DefaultApi.md#petsget) | **GET** /pets | 


<a name="petsget"></a>
# **PetsGet**
> List<Pet> PetsGet ()



Returns all pets from the system that the user has access to

### Example
```csharp
using System;
using System.Diagnostics;
using Org.OpenAPITools.Api;
using Org.OpenAPITools.Client;
using Org.OpenAPITools.Model;

namespace Example
{
    public class PetsGetExample
    {
        public void main()
        {
            var apiInstance = new DefaultApi();

            try
            {
                List&lt;Pet&gt; result = apiInstance.PetsGet();
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling DefaultApi.PetsGet: " + e.Message );
            }
        }
    }
}
```

### Parameters
This endpoint does not need any parameter.

### Return type

[**List<Pet>**](Pet.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

