namespace DemoApi;

internal class ItemDb : DbContext
{
    public ItemDb(DbContextOptions<ItemDb> options)
        : base(options)
    {
    }

    public DbSet<Item> Items => Set<Item>();

    public static async Task<IResult> GetAllItems(ItemDb db) =>
        TypedResults.Ok(await db.Items.ToArrayAsync());

    public static async Task<IResult> GetCompleteItems(ItemDb db) =>
        TypedResults.Ok(await db.Items.Where(e => e.Status == true).ToListAsync());

    public static async Task<IResult> GetItem(ItemDb db, int id) =>
        await db.Items.FindAsync(id) is { } item
            ? TypedResults.Ok(item)
            : TypedResults.NotFound();

    public static async Task<IResult> CreateItem(ItemDb db, Item item)
    {
        db.Items.Add(item);

        await db.SaveChangesAsync();

        return TypedResults.Created($"/{InMemoryDbModule.ItemRoute}/{item.Id}", item);
    }

    public static async Task<IResult> UpdateItem(ItemDb db, int id, Item inputItem)
    {
        var item = await db.Items.FindAsync(id);
        if (item is null)
        {
            return TypedResults.NotFound();
        }

        var updatedItem = item with { Name = inputItem.Name, Status = inputItem.Status };
        db.Update(updatedItem);

        await db.SaveChangesAsync();

        return TypedResults.NoContent();
    }

    public static async Task<IResult> DeleteItem(ItemDb db, int id)
    {
        if (await db.Items.FindAsync(id) is not { } item)
        {
            return TypedResults.NotFound();
        }

        db.Items.Remove(item);

        await db.SaveChangesAsync();

        return TypedResults.Ok(item);
    }
}
