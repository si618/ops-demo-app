namespace DemoApi.Infrastructure;

public interface IEndpointDefinition
{
    string RoutePrefix { get; }

    IServiceCollection DefineServices(IServiceCollection services);

    void DefineEndpoints(WebApplication app);
}
