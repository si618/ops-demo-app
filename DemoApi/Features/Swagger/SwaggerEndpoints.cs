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
        app.UseSwaggerUI();

        // Redirect root to swagger
        app.MapGet("/", () => Results.Redirect($"/{RoutePrefix}"))
            .ExcludeFromDescription();
    }
}
