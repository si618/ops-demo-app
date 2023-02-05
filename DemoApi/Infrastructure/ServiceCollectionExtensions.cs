namespace DemoApi.Infrastructure;

using System.Collections.ObjectModel;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Register IEndpointDefinition services (hat-tip: Nick Chapsas 🙇‍♂️)
    /// </summary>
    public static void AddEndpointsInAssembly(this IServiceCollection services)
    {
        var types = Assembly
            .GetExecutingAssembly()
            .ExportedTypes
            .Where(t => typeof(IEndpointDefinition).IsAssignableFrom(t) && t.IsClass)
            .Select(Activator.CreateInstance)
            .Cast<IEndpointDefinition>()
            .ToList();

        var endpointDefinitions = new ReadOnlyCollection<IEndpointDefinition>(types);

        foreach (var endpointDefinition in endpointDefinitions)
        {
            endpointDefinition.DefineServices(services);
        }

        services.AddSingleton((endpointDefinitions as IReadOnlyCollection<IEndpointDefinition>)!);
    }
}
