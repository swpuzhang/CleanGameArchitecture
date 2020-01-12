using Commons.Enums;
using Commons.Tools.Encryption;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commons.AspMiddlewares
{
     class MiddleResponse
    {
        public MiddleResponse()
        {
        }

        [JsonConstructor]
        public MiddleResponse(ResponseStatus statusCode, List<string> errorInfos)
        {
            StatusCode = statusCode;
            ErrorInfos = errorInfos;
        }

        public ResponseStatus StatusCode { get; private set; }
        public List<string> ErrorInfos { get; private set; }
    }

    public class TokenCheckMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly List<string> _filterApis;

        public TokenCheckMiddleware(RequestDelegate next, List<string> filterApis)
        {
            _next = next;
            _filterApis = filterApis;
        }

        public Task InvokeAsync(HttpContext context)
        {
            //可选参数始终不为null
            string findPath = _filterApis.FirstOrDefault(x => x == context.Request.Path);
            if (!string.IsNullOrEmpty(findPath))
            {
                return _next(context);
            }

            var token = context.Request.Headers["Token"];
            if (string.IsNullOrEmpty(token))
            {
                return context.Response.WriteAsync(JsonConvert.SerializeObject(
                    new MiddleResponse(ResponseStatus.TokenError, null)));
                ;

            }

            var status = TokenTool.ParseToken(token, out var id);
            if (status != ResponseStatus.Success)
            {
                return context.Response.WriteAsync(JsonConvert.SerializeObject(
                    new MiddleResponse(status, null)));
            }
            context.Request.Headers["id"] = id.ToString();
            return _next(context);
        }

       

    }
    public static class TokenCheckMiddlewareExtenssion
    {
        public static IApplicationBuilder UseTokenCheck(
           this IApplicationBuilder builder, params string[] filterApi)
        {
            return builder.UseMiddleware<TokenCheckMiddleware>(filterApi.ToList());
        }
    }
}
