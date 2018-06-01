/*
 * Swagger Petstore
 *
 * A sample API that uses a petstore as an example to demonstrate features in the swagger-2.0 specification
 *
 * OpenAPI spec version: 1.0.0
 * 
 * Generated by: https://openapi-generator.tech
 */

using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Http;
using Org.OpenAPITools.Attributes;
using Org.OpenAPITools.Models;

namespace Org.OpenAPITools.Controllers
{ 
    public class DefaultApiController : Controller
    { 
        private static string SessionIdToken = "session-id";
        
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Returns all pets from the system that the user has access to</remarks>
        /// <response code="200">A list of pets.</response>
        [HttpGet]
        [Route("/api/pets")]
        [ValidateModelState]
        [SwaggerOperation("PetsGet")]
        [SwaggerResponse(statusCode: 200, type: typeof(List<Pet>), description: "A list of pets.")]
        public virtual IActionResult PetsGet()
        { 
            String cookie = null;
            if (!HttpContext.Request.Cookies.TryGetValue(SessionIdToken, out cookie))
            {
                cookie = Guid.NewGuid().ToString("N");
            }
            
            HttpContext.Response.Cookies.Append(
                SessionIdToken,
                cookie,
                new CookieOptions
                {
                    HttpOnly = true
                });

            var pets = new List<Pet>
            {
                new Pet {Id = 1, Name = "Jim", Tag = "Person"},
                new Pet {Id = 2, Name = "Socks", Tag = "Cat"},
                new Pet {Id = 3, Name = "Fido", Tag = "Dog"},
                new Pet {Id = 4, Name = "Pookie", Tag = "Snake"}
            };
            return new ObjectResult(pets);
        }
    }
}
