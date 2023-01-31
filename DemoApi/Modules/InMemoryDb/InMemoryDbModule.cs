namespace DemoApi.Modules.Items;

internal static class InMemoryDbModule
{
    public const string ItemRoute = "item";

    public static IServiceCollection RegisterInMemoryDbModule(this IServiceCollection services)
    {
        services.AddDbContext<ItemDb>(opt => opt.UseInMemoryDatabase("Items"));
        return services;
    }

    public static IEndpointRouteBuilder MapInMemoryDbEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var item = endpoints.MapGroup($"/{ItemRoute}")
            .WithTags("In-Memory Database");

        item.MapGet("/", ItemDb.GetAllItems)
            .Produces<Item>();

        item.MapGet("/complete", ItemDb.GetCompleteItems)
            .Produces<Item>();

        item.MapGet("/{id:int}", ItemDb.GetItem)
            .Produces<Item>()
            .Produces(StatusCodes.Status404NotFound);

        item.MapPost("/", ItemDb.CreateItem)
            .Produces(StatusCodes.Status201Created);

        item.MapPut("/{id:int}", ItemDb.UpdateItem)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);

        item.MapDelete("/{id:int}", ItemDb.DeleteItem)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        return endpoints;
    }
}
