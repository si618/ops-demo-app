namespace DemoHttp.Services.Kubernetes;

using Infrastructure;

public class KubeEndpoints : IEndpointDefinition
{
    public string RoutePrefix => "kube";

    public IServiceCollection DefineServices(IServiceCollection services)
    {
        return services;
    }

    public void DefineEndpoints(WebApplication app)
    {
        // TODO Demo kube integration & resilience
    }
}
