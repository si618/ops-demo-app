namespace DemoApi.Features.Swagger;

using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Runtime.Serialization;

/// <summary>
/// Hat-tip: Andrew Varnon 🙇‍♂️
/// https://avarnon.medium.com/how-to-show-enums-as-strings-in-swashbuckle-aspnetcore-628d0cc271e6
/// </summary>
public sealed class EnumSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema model, SchemaFilterContext context)
    {
        if (!context.Type.IsEnum)
        {
            return;
        }

        model.Enum.Clear();
        foreach (var enumName in Enum.GetNames(context.Type))
        {
            var memberInfo = context.Type
                .GetMember(enumName)
                .FirstOrDefault(m => m.DeclaringType == context.Type);

            var enumMemberAttribute = memberInfo?
                .GetCustomAttributes(typeof(EnumMemberAttribute), false)
                .OfType<EnumMemberAttribute>()
                .FirstOrDefault();

            var label = enumMemberAttribute == null || string.IsNullOrWhiteSpace(enumMemberAttribute.Value)
                ? enumName
                : enumMemberAttribute.Value;

            model.Enum.Add(new OpenApiString(label));
        }
    }
}
