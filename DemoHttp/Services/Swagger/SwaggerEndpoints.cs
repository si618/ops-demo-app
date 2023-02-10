namespace DemoHttp.Services.Swagger;

using Infrastructure;

public class SwaggerEndpoints : IEndpointDefinition
{
    public string RoutePrefix => "swagger";

    public IServiceCollection DefineServices(IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c => c.SchemaFilter<EnumSchemaFilter>());

        return services;
    }

    public void DefineEndpoints(WebApplication app)
    {
        const string swaggerCss = "SwaggerDark.css";

        app.UseSwagger();

        // Hat-tip: Romans Pokrovskis 🙇‍♂️ https://github.com/Amoenus/SwaggerDark/
        app.UseSwaggerUI(opt => opt.InjectStylesheet($"/swagger-ui/{swaggerCss}"));

        // Redirect root to swagger
        app.MapGet("/", () => Results.Redirect($"/{RoutePrefix}"))
            .ExcludeFromDescription();

        app.MapGet($"/swagger-ui/{swaggerCss}",
        async (CancellationToken cancellationToken) =>
        {
            // Workaround for published app vs locally run project
            var path = File.Exists(swaggerCss)
                ? swaggerCss
                : Path.Combine("Services", "Swagger", swaggerCss);

            var css = await File.ReadAllBytesAsync(path, cancellationToken);

            return Results.File(css, "text/css");
        }).ExcludeFromDescription();
    }
}
