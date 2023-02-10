namespace DemoHttp.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static void AddDatabasesInAssembly(this IServiceCollection services)
    {
        // TODO Split out from IEndpointDefinition
        var databases = Assembly
            .GetExecutingAssembly()
            .ExportedTypes
            .Where(type => typeof(IEndpointDefinition).IsAssignableFrom(type) &&
                           type is { IsClass: true, IsAbstract: false })
            .Select(Activator.CreateInstance)
            .Cast<IEndpointDefinition>()
            .ToImmutableList();

        foreach (var database in databases)
        {
            database.DefineServices(services);
        }
    }

    public static void ConfigureJsonOptions(this IServiceCollection services)
    {
        services.Configure<JsonOptions>(options =>
        {
            options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
            options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            options.SerializerOptions.WriteIndented = true;
        });
    }
}
