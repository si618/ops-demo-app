namespace DemoApi.Services.Crud;

public interface IItemDb
{
    /// <summary>
    /// Retrieves all items
    /// </summary>
    /// <returns>Array of <see cref="Item"/></returns>
    ValueTask<Item[]> GetItems();

    /// <summary>
    /// Retrieves items with specified <paramref name="status"/>
    /// </summary>
    /// <returns>Array of <see cref="Item"/> matching status</returns>
    ValueTask<Item[]> GetItemsWithStatus(ItemStatus status);

    /// <summary>
    /// Retrieves item with id <paramref name="id"/>
    /// </summary>
    /// <param name="id">The id of the item to retrieve</param>
    /// <returns><see cref="Item"/> if found; otherwise <c>null</c></returns>
    ValueTask<Item?> GetItem(Guid id);

    /// <summary>
    /// Seed random items
    /// </summary>
    /// <param name="count">The number of items to create</param>
    ValueTask SeedItems(int count);

    /// <summary>
    /// Creates specified <paramref name="item"/>
    /// </summary>
    /// <param name="item">The item to create</param>
    ValueTask CreateItem(Item item);

    /// <summary>
    /// Updates specified <paramref name="item"/>
    /// </summary>
    /// <param name="item">The item to update</param>
    ValueTask UpdateItem(Item item);

    /// <summary>
    /// Deletes item with id <paramref name="id"/>
    /// </summary>
    /// <param name="id">The id of the item to delete</param>
    ValueTask DeleteItem(Guid id);
}
