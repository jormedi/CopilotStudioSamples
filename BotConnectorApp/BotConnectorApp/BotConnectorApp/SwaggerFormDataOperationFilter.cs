using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

public class SwaggerFormDataOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var formParameters = context.ApiDescription.ParameterDescriptions
            .Where(p => p.Source == Microsoft.AspNetCore.Mvc.ModelBinding.BindingSource.Form)
            .ToList();

        if (formParameters.Any())
        {
            operation.RequestBody = new OpenApiRequestBody
            {
                Content =
                {
                    ["application/x-www-form-urlencoded"] = new OpenApiMediaType
                    {
                        Schema = new OpenApiSchema
                        {
                            Type = "object",
                            Properties = formParameters.ToDictionary(
                                p => p.Name,
                                p => new OpenApiSchema { Type = "string" }
                            )
                        }
                    }
                }
            };
        }
    }
}
