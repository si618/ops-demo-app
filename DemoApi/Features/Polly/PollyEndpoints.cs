namespace DemoApi.Features.Polly;

public class PollyEndpoints : IEndpointDefinition
{
    public string RoutePrefix => "polly";

    public IServiceCollection DefineServices(IServiceCollection services)
    {
        return services;
    }

    public void DefineEndpoints(WebApplication app)
    {
        // TODO demo polly integration & API resilience
    }
}
