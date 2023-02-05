namespace DemoApi.Infrastructure;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Register IEndpointDefinition services (hat-tip: Nick Chapsas 🙇‍♂️)
    /// </summary>
    public static void AddEndpointsInAssembly(this IServiceCollection services)
    {
        var endpointDefinitions = Assembly
            .GetExecutingAssembly()
            .ExportedTypes
            .Where(t => typeof(IEndpointDefinition).IsAssignableFrom(t) && t.IsClass)
            .Select(Activator.CreateInstance)
            .Cast<IEndpointDefinition>()
            .ToImmutableList();

        foreach (var endpointDefinition in endpointDefinitions)
        {
            endpointDefinition.DefineServices(services);
        }

        services.AddSingleton(endpointDefinitions);
    }
}
