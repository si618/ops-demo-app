namespace DemoHttp.Test;

public class ItemEndpointsTests
{
    private readonly ItemEndpoints _api;
    private readonly Mock<IItemDb> _db;
    private readonly Item _item;

    public ItemEndpointsTests()
    {
        _api = new ItemEndpoints();
        _db = new Mock<IItemDb>();
        _item = new Item { Id = Guid.NewGuid(), Name = "Name of Item" };
    }

    [Fact]
    public async Task SeedItems_ReturnsStatus200OK()
    {
        var result = await ItemEndpoints.SeedItems(_db.Object, 10);

        result.Should().Be(TypedResults.Ok());
    }

    [Fact]
    public async Task GetItems_WhenItemsFound_ReturnsStatus200OK()
    {
        var items = new[] { _item, new() { Name = "Name of another item" } };
        _db.Setup(db => db.GetItems()).ReturnsAsync(items);

        var result = await ItemEndpoints.GetItems(_db.Object);

        result.Should().BeEquivalentTo(TypedResults.Ok(items));
    }

    [Fact]
    public async Task GetItems_WhenNoItemsFound_ReturnsStatus200OK()
    {
        var result = await ItemEndpoints.GetItems(_db.Object);

        result.Should().BeEquivalentTo(TypedResults.Ok(Array.Empty<Item>()));
    }

    [Fact]
    public async Task GetItemsWithStatus_WhenItemsWithStatusFound_ReturnsStatus200OK()
    {
        const ItemStatus status = ItemStatus.Complete;
        var items = new[] { _item };
        _db.Setup(db => db.GetItemsWithStatus(status)).ReturnsAsync(items);

        var result = await ItemEndpoints.GetItemsWithStatus(_db.Object, status);

        result.Should().BeEquivalentTo(TypedResults.Ok(items));
    }

    [Fact]
    public async Task GetItemsWithStatus_WhenNoItemsWithStatusFound_ReturnsStatus200OK()
    {
        var result = await ItemEndpoints.GetItemsWithStatus(_db.Object, ItemStatus.Complete);

        result.Should().BeEquivalentTo(TypedResults.Ok(Array.Empty<Item>()));
    }

    [Fact]
    public async Task GetItem_WhenItemFound_ReturnsStatus200OK()
    {
        _db.Setup(db => db.GetItem(_item.Id)).ReturnsAsync(_item);

        var result = await ItemEndpoints.GetItem(_db.Object, _item.Id);

        result.Should().BeEquivalentTo(TypedResults.Ok(_item));
    }

    [Fact]
    public async Task GetItem_WhenItemNotFound_ReturnsStatus404NotFound()
    {
        var result = await ItemEndpoints.GetItem(_db.Object, Guid.Empty);

        result.Should().Be(TypedResults.NotFound());
    }

    [Fact]
    public async Task CreateItem_WhenItemFound_ReturnsStatus409Conflict()
    {
        _db.Setup(db => db.GetItem(_item.Id)).ReturnsAsync(_item);

        var result = await _api.CreateItem(_db.Object, _item);

        result.Should().Be(TypedResults.Conflict());
    }

    [Fact]
    public async Task CreateItem_WhenItemNotFound_ReturnsStatus201Created()
    {
        var result = await _api.CreateItem(_db.Object, _item);

        result.Should()
            .BeEquivalentTo(TypedResults.Created($"/{_api.RoutePrefix}/{_item.Id}", _item));
    }

    [Fact]
    public async Task UpdateItem_WhenItemFound_ReturnsStatus204NoContent()
    {
        _db.Setup(db => db.GetItem(_item.Id)).ReturnsAsync(_item);

        var result = await ItemEndpoints.UpdateItem(_db.Object, _item);

        result.Should().Be(TypedResults.NoContent());
    }

    [Fact]
    public async Task UpdateItem_WhenItemNotFound_ReturnsStatus404NotFound()
    {
        var result = await ItemEndpoints.UpdateItem(_db.Object, _item);

        result.Should().Be(TypedResults.NotFound());
    }

    [Fact]
    public async Task DeleteItem_WhenItemFound_ReturnsStatus204NoContent()
    {
        _db.Setup(db => db.GetItem(_item.Id)).ReturnsAsync(_item);

        var result = await ItemEndpoints.DeleteItem(_db.Object, _item.Id);

        result.Should().Be(TypedResults.NoContent());
    }

    [Fact]
    public async Task DeleteItem_WhenItemNotFound_ReturnsStatus404NotFound()
    {
        var result = await ItemEndpoints.DeleteItem(_db.Object, _item.Id);

        result.Should().Be(TypedResults.NotFound());
    }
}
