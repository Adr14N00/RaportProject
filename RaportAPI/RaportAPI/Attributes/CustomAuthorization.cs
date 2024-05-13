using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using SelfOnBoardingManagement.Infrastructure.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using System.Web.Providers.Entities;

namespace SelfOnboardingManagement.WebApi.Attributes
{
    public class CustomAuthorization : Attribute, IAuthorizationFilter
    {

        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {
            var allowAnonymous = filterContext.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
            if (allowAnonymous)
                return;

            Microsoft.Extensions.Primitives.StringValues authTokens;
            filterContext.HttpContext.Request.Headers.TryGetValue("Authorization", out authTokens);

            var _token = authTokens.FirstOrDefault();

            if (_token != null)
            {
                if (IsValidToken(filterContext))
                {
                    filterContext.HttpContext.Response.Headers.Add("Authorization", _token);
                    filterContext.HttpContext.Response.Headers.Add("AuthStatus", "Authorized");

                    filterContext.HttpContext.Response.Headers.Add("storeAccessiblity", "Authorized");

                    return;
                }
                else
                {
                    filterContext.HttpContext.Response.Headers.Add("Authorization", _token);
                    filterContext.HttpContext.Response.Headers.Add("AuthStatus", "NotAuthorized");

                    filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    filterContext.HttpContext.Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "Not Authorized";
                    filterContext.Result = new JsonResult("NotAuthorized")
                    {
                        Value = new
                        {
                            Status = "Error",
                            Message = "Invalid Token"
                        },
                    };
                }


            }
            else
            {
                Microsoft.Extensions.Primitives.StringValues apikey;
                var key = filterContext.HttpContext.Request.Headers.TryGetValue("x-api-key", out apikey);
                var keyvalue = apikey.FirstOrDefault();

                if(IsValidApiKey(keyvalue, filterContext))
                {
                    filterContext.HttpContext.Response.Headers.Add("ApiKey", keyvalue);
                    filterContext.HttpContext.Response.Headers.Add("AuthStatus", "Authorized");

                    filterContext.HttpContext.Response.Headers.Add("storeAccessiblity", "Authorized");

                    return;
                }


                filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                filterContext.HttpContext.Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "Please Provide API Key";
                filterContext.Result = new JsonResult("Please Provide API Key")
                {
                    Value = new
                    {
                        Status = "Error",
                        Message = "Please Provide API key"
                    },
                };
            }
            
        }

        private bool IsValidToken(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals("UserID")); 
            if (user == null) 
            {
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
                return false;
            }
            return true;
        }

        private bool IsValidApiKey(string? key, AuthorizationFilterContext context)
        {
            if (string.IsNullOrWhiteSpace(key)) return false;

            var configuration = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();

            var apiKey = configuration.GetValue<string>("ApiKeyAuth:ApiKey");
            if (!apiKey.Equals(key)) return false;
            
            return true;
        }
    }
}
