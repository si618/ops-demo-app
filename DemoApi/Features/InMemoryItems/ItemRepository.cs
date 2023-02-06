namespace DemoApi.Features.InMemoryItems;

internal sealed class ItemRepository : IItemRepository
{
    private readonly ItemDb _db;

    public ItemRepository(ItemDb db)
    {
        _db = db;
    }

    public async Task<Item[]> GetAllItems() => await _db.Items.AsNoTracking().ToArrayAsync();

    public async Task<Item[]> GetItems(ItemStatus status) =>
        await _db.Items
            .AsNoTracking()
            .Where(e => e.Status == status)
            .ToArrayAsync();

    public async Task<Item?> GetItem(Guid id) => await _db.Items.FindAsync(id);

    public async Task<bool> CreateItem(Item item)
    {
        if (await _db.Items.FindAsync(item.Id) is not null)
        {
            return false;
        }

        _db.Items.Add(item);
        await _db.SaveChangesAsync();

        return true;
    }

    public async Task<bool> UpdateItem(Item itemDto)
    {
        var item = await _db.Items.FindAsync(itemDto.Id);
        if (item is null)
        {
            return false;
        }

        var updatedItem = item with { Name = itemDto.Name, Status = itemDto.Status };
        _db.Update(updatedItem);

        await _db.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteItem(Guid id)
    {
        var item = await _db.Items.FindAsync(id);
        if (item is null)
        {
            return false;
        }

        _db.Items.Remove(item);
        await _db.SaveChangesAsync();

        return true;
    }
}