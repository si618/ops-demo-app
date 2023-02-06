namespace DemoApi.Features.InMemoryItems;

public interface IItemRepository
{
    /// <summary>
    /// Retrieves all items
    /// </summary>
    /// <returns>Array of <see cref="Item"/></returns>
    Task<Item[]> GetAllItems();

    /// <summary>
    /// Retrieves items with specified <paramref name="status"/>
    /// </summary>
    /// <returns>Array of <see cref="Item"/> matching status</returns>
    Task<Item[]> GetItems(ItemStatus status);

    /// <summary>
    /// Retrieves item with id <paramref name="id"/>
    /// </summary>
    /// <param name="id">The id of the item to retrieve</param>
    /// <returns><see cref="Item"/> if found; otherwise <c>null</c></returns>
    Task<Item?> GetItem(Guid id);

    /// <summary>
    /// Creates specified <paramref name="item"/>
    /// </summary>
    /// <param name="item">The item to create</param>
    /// <returns><c>true</c> if item was created; otherwise <c>false</c></returns>
    Task<bool> CreateItem(Item item);

    /// <summary>
    /// Updates specified <paramref name="item"/>
    /// </summary>
    /// <param name="item">The item to update</param>
    /// <returns><c>true</c> if item was updated; otherwise <c>false</c></returns>
    Task<bool> UpdateItem(Item item);

    /// <summary>
    /// Deletes item with id <paramref name="id"/>
    /// </summary>
    /// <param name="id">The id of the item to delete</param>
    /// <returns><c>true</c> if item was deleted; otherwise <c>false</c></returns>
    Task<bool> DeleteItem(Guid id);
}
