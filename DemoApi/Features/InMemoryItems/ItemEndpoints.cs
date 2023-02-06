namespace DemoApi.Features.InMemoryItems;

public class ItemEndpoints : IEndpointDefinition
{
    public string RoutePrefix => "item";

    public IServiceCollection DefineServices(IServiceCollection services)
    {
        services.AddDbContext<ItemDb>(opt => opt.UseInMemoryDatabase("Items"));
        services.AddScoped<IItemRepository, ItemRepository>();

        return services;
    }

    public void DefineEndpoints(WebApplication app)
    {
        var routeGroup = app
            .MapGroup($"/{RoutePrefix}")
            .WithTags("In-Memory Items");

        routeGroup.MapGet("/", GetAllItems)
            .Produces<Item>();

        routeGroup.MapGet("/status/{status}", GetItems)
            .Produces<Item>();

        routeGroup.MapGet("/{id:guid}", GetItem)
            .Produces<Item>()
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        routeGroup.MapPost("/", CreateItem)
            .Produces(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status409Conflict);

        routeGroup.MapPut("/{item}", UpdateItem)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);

        routeGroup.MapDelete("/{id:guid}", DeleteItem)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);
    }

    public static async Task<IResult> GetAllItems(IItemRepository repository) =>
        TypedResults.Ok(await repository.GetAllItems());

    public static async Task<IResult> GetItems(IItemRepository repository, ItemStatus status) =>
        TypedResults.Ok(await repository.GetItems(status));

    public static async Task<IResult> GetItem(IItemRepository repository, Guid id) =>
        await repository.GetItem(id) is { } item
            ? TypedResults.Ok(item)
            : TypedResults.NotFound();

    public async Task<IResult> CreateItem(IItemRepository repository, Item item) =>
        await repository.CreateItem(item)
            ? TypedResults.Created($"/{RoutePrefix}/{item.Id}", item)
            : TypedResults.Conflict();

    public static async Task<IResult> UpdateItem(IItemRepository repository, Item item) =>
        await repository.UpdateItem(item) ? TypedResults.NoContent() : TypedResults.NotFound();

    public static async Task<IResult> DeleteItem(IItemRepository repository, Guid id) =>
        await repository.DeleteItem(id) ? TypedResults.NoContent() : TypedResults.NotFound();
}