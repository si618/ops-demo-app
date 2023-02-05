namespace DemoApi.Test;

public class ItemEndpointsTest
{
    [Fact]
    public async Task GetAllItems_WhenItemsExist_ReturnsStatus200OK()
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
    public async Task GetAllItems_WhenNoItemsExist_ReturnsStatus200OK()
    {
        var itemRepository = new Mock<IItemRepository>();
        var items = Array.Empty<Item>();

        var result = await ItemEndpoints.GetAllItems(itemRepository.Object);

        result.Should().BeEquivalentTo(TypedResults.Ok(items));
    }

}
