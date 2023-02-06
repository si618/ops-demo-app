namespace DemoApi.Test;

/// <summary>
/// Tests to verify endpoints are returning the expected status codes result
/// </summary>
public class ItemEndpointsTests
{
    [Fact]
    public async Task GetAllItems_WhenItemsFound_ReturnsStatus200OK()
    {
        var items = new Item[] { new() { Name = "Item1" }, new() { Name = "Item2" } };
        var itemRepository = new Mock<IItemRepository>();
        itemRepository
            .Setup(repo => repo.GetAllItems())
            .ReturnsAsync(items);

        var result = await ItemEndpoints.GetAllItems(itemRepository.Object);

        result.Should().BeEquivalentTo(TypedResults.Ok(items));
    }

    [Fact]
    public async Task GetAllItems_WhenNoItemsFound_ReturnsStatus200OK()
    {
        var itemRepository = new Mock<IItemRepository>();
        var items = Array.Empty<Item>();

        var result = await ItemEndpoints.GetAllItems(itemRepository.Object);

        result.Should().BeEquivalentTo(TypedResults.Ok(items));
    }

    [Fact]
    public async Task GetItems_WhenItemsWithStatusFound_ReturnsStatus200OK()
    {
        const ItemStatus status = ItemStatus.Complete;
        var items = new Item[] { new() { Name = "Item1", Status = status } };
        var itemRepository = new Mock<IItemRepository>();
        itemRepository
            .Setup(repo => repo.GetItems(status))
            .ReturnsAsync(items);

        var result = await ItemEndpoints.GetItems(itemRepository.Object, status);

        result.Should().BeEquivalentTo(TypedResults.Ok(items));
    }

    [Fact]
    public async Task GetItems_WhenNoItemsWithStatusFound_ReturnsStatus200OK()
    {
        var itemRepository = new Mock<IItemRepository>();
        var items = Array.Empty<Item>();

        var result = await ItemEndpoints.GetItems(itemRepository.Object, ItemStatus.Complete);

        result.Should().BeEquivalentTo(TypedResults.Ok(items));
    }

    [Fact]
    public async Task GetItem_WhenItemFound_ReturnsStatus200OK()
    {
        var id = Guid.NewGuid();
        var item = new Item { Id = id, Name = "Item1" };
        var itemRepository = new Mock<IItemRepository>();
        itemRepository
            .Setup(repo => repo.GetItem(id))
            .ReturnsAsync(item);

        var result = await ItemEndpoints.GetItem(itemRepository.Object, id);

        result.Should().BeEquivalentTo(TypedResults.Ok(item));
    }

    [Fact]
    public async Task GetItem_WhenItemNotFound_ReturnsStatus404NotFound()
    {
        var itemRepository = new Mock<IItemRepository>();

        var result = await ItemEndpoints.GetItem(itemRepository.Object, Guid.Empty);

        result.Should().BeEquivalentTo(TypedResults.NotFound());
    }

    [Fact]
    public async Task CreateItem_WhenItemFound_ReturnsStatus409Conflict()
    {
        var item = new Item { Name = "Item1" };
        var itemRepository = new Mock<IItemRepository>();
        itemRepository
            .Setup(repo => repo.CreateItem(item))
            .ReturnsAsync(false);

        var result = await new ItemEndpoints().CreateItem(itemRepository.Object, item);

        result.Should().BeEquivalentTo(TypedResults.Conflict());
    }

    [Fact]
    public async Task CreateItem_WhenItemNotFound_ReturnsStatus200OK()
    {
        var item = new Item { Id = Guid.NewGuid(), Name = "Item1" };
        var itemRepository = new Mock<IItemRepository>();
        itemRepository
            .Setup(repo => repo.CreateItem(item))
            .ReturnsAsync(true);
        var itemEndpoints = new ItemEndpoints();

        var result = await itemEndpoints.CreateItem(itemRepository.Object, item);

        result.Should().BeEquivalentTo(
            TypedResults.Created($"/{itemEndpoints.RoutePrefix}/{item.Id}", item));
    }

    [Fact]
    public async Task UpdateItem_WhenItemFound_ReturnsStatus204NoContent()
    {
        var item = new Item { Name = "Item1" };
        var itemRepository = new Mock<IItemRepository>();
        itemRepository
            .Setup(repo => repo.UpdateItem(item))
            .ReturnsAsync(true);

        var result = await ItemEndpoints.UpdateItem(itemRepository.Object, item);

        result.Should().BeEquivalentTo(TypedResults.NoContent());
    }

    [Fact]
    public async Task UpdateItem_WhenItemNotFound_ReturnsStatus404NotFound()
    {
        var item = new Item { Id = Guid.NewGuid(), Name = "Item1" };
        var itemRepository = new Mock<IItemRepository>();
        itemRepository
            .Setup(repo => repo.UpdateItem(item))
            .ReturnsAsync(false);

        var result = await ItemEndpoints.UpdateItem(itemRepository.Object, item);

        result.Should().BeEquivalentTo(TypedResults.NotFound());
    }

    [Fact]
    public async Task DeleteItem_WhenItemFound_ReturnsStatus204NoContent()
    {
        var item = new Item { Id = Guid.NewGuid(), Name = "Item1" };
        var itemRepository = new Mock<IItemRepository>();
        itemRepository
            .Setup(repo => repo.DeleteItem(item.Id))
            .ReturnsAsync(true);

        var result = await ItemEndpoints.DeleteItem(itemRepository.Object, item.Id);

        result.Should().BeEquivalentTo(TypedResults.NoContent());
    }

    [Fact]
    public async Task DeleteItem_WhenItemNotFound_ReturnsStatus404NotFound()
    {
        var item = new Item { Id = Guid.NewGuid(), Name = "Item1" };
        var itemRepository = new Mock<IItemRepository>();
        itemRepository
            .Setup(repo => repo.DeleteItem(item.Id))
            .ReturnsAsync(false);

        var result = await ItemEndpoints.DeleteItem(itemRepository.Object, item.Id);

        result.Should().BeEquivalentTo(TypedResults.NotFound());
    }
}
