namespace DemoApi.Features.Kubernetes;

public class KubeEndpoints : IEndpointDefinition
{
    public string RoutePrefix => "kube";

    public IServiceCollection DefineServices(IServiceCollection services)
    {
        return services;
    }

    public void DefineEndpoints(WebApplication app)
    {
        // TODO demo kube integration & resilience
    }
}
