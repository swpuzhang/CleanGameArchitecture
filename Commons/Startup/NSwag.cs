using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.DotNet.PlatformAbstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSwag;
using NSwag.AspNetCore;
using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Commons.Startup
{
    public class AddRequiredHeaderParameter : IOperationProcessor
    {
        public bool Process(OperationProcessorContext context)
        {
            context.OperationDescription.Operation.Parameters.Add(
            new OpenApiParameter
            {
                Name = "Token",
                Kind = OpenApiParameterKind.Header,
                Type = NJsonSchema.JsonObjectType.String,
                IsRequired = false,
                Description = "登录后返回的Token  除了登录接口其他请求必须带上",
                Default = ""
            });
            return true;
        }

    }

    public static class NSwagConfig
    {
        public static void ConfigSwagger(this IServiceCollection services,
            IConfiguration configuration)
        {

            services.AddOpenApiDocument(document =>
            {
                document.DocumentName = configuration["Service:ServiceName"];
                document.OperationProcessors.Add(new AddRequiredHeaderParameter());
            });
        }

        public static void UseSwaggerService(this IApplicationBuilder app)
        {
            app.UseOpenApi();
            app.UseSwaggerUi3();

        }
    }
}
