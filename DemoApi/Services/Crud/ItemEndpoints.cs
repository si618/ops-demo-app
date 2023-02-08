namespace DemoApi.Services.Crud;

public class ItemEndpoints : IEndpointDefinition
{
    public string RoutePrefix => "item";

    public IServiceCollection DefineServices(IServiceCollection services)
    {
        services.AddDbContext<ItemDbInMemory>();
        services.AddScoped<IItemDb, ItemDbInMemory>();

        return services;
    }

    public void DefineEndpoints(WebApplication app)
    {
        var routeGroup = app
            .MapGroup($"/{RoutePrefix}")
            .WithTags("In-Memory Item CRUD");

        routeGroup.MapGet("/all", GetItems)
            .Produces<Item>();

        routeGroup.MapGet("/status/{status}", GetItemsWithStatus)
            .Produces<Item>();

        routeGroup.MapGet("/{id:guid}", GetItem)
            .Produces<Item>()
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        routeGroup.MapPost("/seed/{count:int}", SeedItems);

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

    public static async ValueTask<IResult> SeedItems(IItemDb db, int count)
    {
        await db.SeedItems(count);
        return TypedResults.Ok();
    }

    public static async ValueTask<IResult> GetItems(IItemDb db) =>
        TypedResults.Ok(await db.GetItems());

    public static async ValueTask<IResult> GetItemsWithStatus(IItemDb db, ItemStatus status) =>
        TypedResults.Ok(await db.GetItemsWithStatus(status));

    public static async ValueTask<IResult> GetItem(IItemDb db, Guid id) =>
        await db.GetItem(id) is { } item
            ? TypedResults.Ok(item)
            : TypedResults.NotFound();

    public async ValueTask<IResult> CreateItem(IItemDb db, Item item)
    {
        if (await db.GetItem(item.Id) is not null)
        {
            return TypedResults.Conflict();
        }

        await db.CreateItem(item);

        return TypedResults.Created($"/{RoutePrefix}/{item.Id}", item);
    }

    public static async ValueTask<IResult> UpdateItem(IItemDb db, Item item)
    {
        if (await db.GetItem(item.Id) is null)
        {
            return TypedResults.NotFound();
        }

        await db.UpdateItem(item);

        return TypedResults.NoContent();
    }

    public static async ValueTask<IResult> DeleteItem(IItemDb db, Guid id)
    {
        if (await db.GetItem(id) is null)
        {
            return TypedResults.NotFound();
        }

        await db.DeleteItem(id);

        return TypedResults.NoContent();
    }
}
