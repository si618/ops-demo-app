namespace DemoHttp.Services.Polly;

using Infrastructure;

public class PollyEndpoints : IEndpointDefinition
{
    public string RoutePrefix => "polly";

    public IServiceCollection DefineServices(IServiceCollection services)
    {
        return services;
    }

    public void DefineEndpoints(WebApplication app)
    {
        // TODO Demo polly integration & API resilience
    }
}
