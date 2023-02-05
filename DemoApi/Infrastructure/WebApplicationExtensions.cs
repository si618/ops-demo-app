namespace DemoApi.Infrastructure;

public static class WebApplicationExtensions
{
    /// <summary>
    /// Define all IEndpointDefinition endpoints (hat-tip: Nick Chapsas 🙇‍♂️)
    /// </summary>
    public static void UseEndpointsInAssembly(this WebApplication app)
    {
        var endpointDefinitions = app.Services.GetRequiredService<ImmutableList<IEndpointDefinition>>();

        foreach (var endpointDefinition in endpointDefinitions)
        {
            endpointDefinition.DefineEndpoints(app);
        }
    }
}
