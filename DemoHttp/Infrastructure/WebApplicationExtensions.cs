namespace DemoHttp.Infrastructure;

public static class WebApplicationExtensions
{
    public static void UseEndpointsInAssembly(this WebApplication app)
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
            endpointDefinition.DefineEndpoints(app);
        }
    }
}
