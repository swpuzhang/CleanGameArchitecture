using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.DotNet.PlatformAbstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Commons.Startup
{

    public class SwaggerAddEnumDescriptions : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            // add enum descriptions to result models
            foreach (var property in swaggerDoc.Components.Schemas.Where(x => x.Value?.Enum?.Count > 0))
            {
                IList<IOpenApiAny> propertyEnums = property.Value.Enum;
                if (propertyEnums != null && propertyEnums.Count > 0)
                {
                    property.Value.Description += DescribeEnum(propertyEnums, property.Key);
                }
            }

            // add enum descriptions to input parameters
            foreach (var pathItem in swaggerDoc.Paths.Values)
            {
                DescribeEnumParameters(pathItem.Operations, swaggerDoc);
            }
        }

        private void DescribeEnumParameters(IDictionary<OperationType, OpenApiOperation> operations, OpenApiDocument swaggerDoc)
        {
            if (operations != null)
            {
                foreach (var oper in operations)
                {
                    foreach (var param in oper.Value.Parameters)
                    {
                        var paramEnum = swaggerDoc.Components.Schemas.FirstOrDefault(x => x.Key == param.Name);
                        if (paramEnum.Value != null)
                        {
                            param.Description += DescribeEnum(paramEnum.Value.Enum, paramEnum.Key);
                        }
                    }
                }
            }
        }

        private Type GetEnumTypeByName(string enumTypeName)
        {
            return AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .FirstOrDefault(x => x.Name == enumTypeName);
        }

        private string DescribeEnum(IList<IOpenApiAny> enums, string proprtyTypeName)
        {
            List<string> enumDescriptions = new List<string>();
            var enumType = GetEnumTypeByName(proprtyTypeName);
            if (enumType == null)
                return null;

            foreach (OpenApiInteger enumOption in enums)
            {
                int enumInt = enumOption.Value;

                enumDescriptions.Add(string.Format("{0} = {1}", enumInt, Enum.GetName(enumType, enumInt)));
            }

            return string.Join(", ", enumDescriptions.ToArray());
        }
    }

    class HttpHeaderFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null) operation.Parameters = new List<OpenApiParameter>();

            var attrs = context.ApiDescription.ActionDescriptor.AttributeRouteInfo;
            if (context.ApiDescription.ActionDescriptor is ControllerActionDescriptor descriptor)
            {
                var actionAttributes = descriptor.MethodInfo.GetCustomAttributes(inherit: true);
                bool isAnonymous = actionAttributes.Any(a => a is AllowAnonymousAttribute);
                //非匿名的方法,链接中添加accesstoken值
                if (!isAnonymous)
                {
                    operation.Parameters.Add(new OpenApiParameter()
                    {

                        Name = "Token",
                        In = ParameterLocation.Header,//query header body path formData
                        Required = false //是否必选
                    });
                }
            }
        }
    }

    public static class Swagger
    {
        public static void ConfigSwagger(this IServiceCollection services,
            IConfiguration configuration)
        {

            services.AddSwaggerGen(c =>
            {
                //c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                c.SwaggerDoc(configuration["Service:ServiceName"], 
                    new OpenApiInfo { Title = configuration["Service:ServiceName"], Version = "v1" });
                c.OperationFilter<HttpHeaderFilter>();
                c.DocumentFilter<SwaggerAddEnumDescriptions>();
                string curPath = Directory.GetCurrentDirectory();
                string dir = "CleanGameArchitecture";
                int index = curPath.LastIndexOf(dir);
                string basePath = curPath.Substring(0, index + dir.Length) + "/work/SwaggerInterface";
                var files = Directory.GetFiles(basePath, "*.xml");
                foreach (var oneFile in files)
                {
                    var xmlPath = Path.Combine(basePath, oneFile);
                    c.IncludeXmlComments(xmlPath, true);
                }

            });
        }

        public static void UseSwaggerService(this IApplicationBuilder app,
            IConfiguration configuration)
        {
            app.UseSwagger(c =>
            {
                c.RouteTemplate = "/{documentName}/swagger.json";
            });
            app.UseSwaggerUI(c =>
            {
                c.ShowExtensions();
                c.EnableValidator(null);
                c.SwaggerEndpoint($"/{configuration["Service:ServiceName"]}/swagger.json",
                    configuration["Service:ServiceName"]);
                c.RoutePrefix = string.Empty;
            });

        }
    }
}
