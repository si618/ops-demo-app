namespace DemoHttp.Test;

public class ItemDbContextTests
{
    private readonly ItemDbBase _db;

    public ItemDbContextTests()
    {
        // TODO Use generics to have single test suite for multiple databases
        _db = new ItemDbInMemory();

        _db.Database.EnsureDeleted();
        _db.Database.EnsureCreated();
    }

    [Fact]
    public async Task SeedItems_WithCountOfZero_SeedsZeroItems()
    {
        await _db.SeedItems(0);

        _db.Items.Should().HaveCount(0);
    }

    [Fact]
    public async Task SeedItems_WithNegativeCount_SeedsZeroItems()
    {
        await _db.SeedItems(-10);

        _db.Items.Should().HaveCount(0);
    }

    [Fact]
    public async Task SeedItems_WithCountOfTen_SeedsTenItems()
    {
        await _db.SeedItems(10);

        _db.Items.Should().HaveCount(10);
    }

    [Fact]
    public async Task GetItems_WhenNoItemsSeeded_ReturnsNoItems()
    {
        var items = await _db.GetItems();

        items.Should().BeEmpty();
    }

    [Fact]
    public async Task GetItems_WhenTenItemsSeeded_ReturnsTenItems()
    {
        await _db.SeedItems(10);

        var items = await _db.GetItems();

        _db.Items.Should().HaveCount(10);
        _db.Items.Should().BeEquivalentTo(items);
    }

    [Fact]
    public async Task GetItems_WhenItemsWithStatusExist_ReturnsItemsWithStatus()
    {
        await _db.SeedItems(100);
        const ItemStatus status = ItemStatus.Complete;

        var items = await _db.GetItemsWithStatus(status);

        var expectation = _db.Items.Where(e => e.Status == status);
        items.Should().BeEquivalentTo(expectation);
    }

    [Fact]
    public async Task GetItem_WhenItemDoesntExist_ReturnsNull()
    {
        await _db.SeedItems(1);

        var item = await _db.GetItem(Guid.NewGuid());

        item.Should().BeNull();
    }

    [Fact]
    public async Task GetItem_WhenItemExists_ReturnsItem()
    {
        await _db.SeedItems(10);
        var id = _db.Items.Skip(5).First().Id;

        var item = await _db.GetItem(id);

        var expectation = await _db.Items.FirstAsync(e => e.Id == id);
        item.Should().BeEquivalentTo(expectation);
    }

    [Fact]
    public async Task CreateItem_WhenItemDoesntExist_CreatesItem()
    {
        var item = new Item { Id = Guid.NewGuid(), Name = "Item1" };

        await _db.CreateItem(item);

        var expectation = await _db.Items.FirstAsync();
        item.Should().BeEquivalentTo(expectation);
    }

    [Fact]
    public async Task CreateItem_WhenItemExists_DoesNothing()
    {
        var item = new Item { Id = Guid.NewGuid(), Name = "Item1" };
        await _db.CreateItem(item);

        var action = async () => await _db.CreateItem(item);

        await action.Should().NotThrowAsync();
        _db.Items.Should().HaveCount(1);
    }

    [Fact]
    public async Task UpdateItem_WhenItemDoesntExist_DoesNothing()
    {
        var item1 = new Item { Id = Guid.NewGuid(), Name = "Item1" };
        var item2 = new Item { Id = Guid.NewGuid(), Name = "Item2" };
        await _db.CreateItem(item1);

        var action = async () => await _db.UpdateItem(item2);

        await action.Should().NotThrowAsync();
        var expectation = await _db.Items.FirstAsync();
        item1.Should().BeEquivalentTo(expectation);
    }

    [Fact]
    public async Task UpdateItem_WhenItemExists_UpdatesItem()
    {
        var item = new Item { Id = Guid.NewGuid(), Name = "Item1", Status = ItemStatus.Unknown };
        var itemToUpdate = item with { Name = "Updated Item1", Status = ItemStatus.Incomplete };
        await _db.CreateItem(item);

        await _db.UpdateItem(itemToUpdate);

        _db.Items.Should().HaveCount(1);
        var expectation = await _db.Items.FirstAsync();
        itemToUpdate.Should().BeEquivalentTo(expectation);
    }

    [Fact]
    public async Task DeleteItem_WhenItemDoesntExist_DoesNothing()
    {
        var action = async () => await _db.DeleteItem(Guid.Empty);

        await action.Should().NotThrowAsync();
    }

    [Fact]
    public async Task DeleteItem_WhenItemExists_DeletesItem()
    {
        var item1 = new Item { Id = Guid.NewGuid(), Name = "Item1" };
        var item2 = new Item { Id = Guid.NewGuid(), Name = "Item2" };
        await _db.CreateItem(item1);
        await _db.CreateItem(item2);

        await _db.DeleteItem(item1.Id);

        _db.Items.Should().HaveCount(1);
        _db.Items.FirstOrDefault(e => e.Id == item1.Id).Should().BeNull();
    }
}
