namespace DemoApi.Features.Swagger;

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
        app.UseSwagger();
        app.UseSwaggerUI(opt => opt.InjectStylesheet("/swagger-ui/SwaggerDark.css"));

        // Redirect root to swagger
        app.MapGet("/", () => Results.Redirect($"/{RoutePrefix}"))
            .ExcludeFromDescription();

        // Hat-tip: Romans Pokrovskis 🙇‍♂️ https://github.com/Amoenus/SwaggerDark/
        app.MapGet("/swagger-ui/SwaggerDark.css", async (CancellationToken cancellationToken) =>
        {
            var css = await File.ReadAllBytesAsync(
                "Features\\Swagger\\SwaggerDark.css",
                cancellationToken);

            return Results.File(css, "text/css");
        }).ExcludeFromDescription();
    }
}
