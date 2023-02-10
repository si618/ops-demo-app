namespace DemoHttp.Services.Crud;

internal abstract class ItemDbBase : DbContext, IItemDb
{
    public DbSet<Item> Items => Set<Item>();

    public async ValueTask<Item[]> GetItems() => await Items.AsNoTracking().ToArrayAsync();

    public async ValueTask<Item[]> GetItemsWithStatus(ItemStatus status) =>
        await Items
            .AsNoTracking()
            .Where(e => e.Status == status)
            .ToArrayAsync();

    public async ValueTask<Item?> GetItem(Guid id) => await Items.FindAsync(id);

    public async ValueTask SeedItems(int count)
    {
        if (count <= 0)
        {
            return;
        }

        var random = new Random(42);
        var values = Enum.GetValues<ItemStatus>();
        ItemStatus RandomStatus() => (ItemStatus)values.GetValue(random.Next(values.Length))!;
        static string GetTicks() => DateTime.Now.Ticks.ToString();

        var items = new List<Item>(count);
        for (var i = 0; i < count; i++)
        {
            items.Add(new Item { Id = Guid.NewGuid(), Name = GetTicks(), Status = RandomStatus() });
        }

        await Items.AddRangeAsync(items);
        await SaveChangesAsync();
    }

    public async ValueTask CreateItem(Item item)
    {
        if (await Items.FindAsync(item.Id) is not null)
        {
            return;
        }

        Items.Add(item);

        await SaveChangesAsync();
    }

    public async ValueTask UpdateItem(Item item)
    {
        var current = await Items.FindAsync(item.Id);
        if (current is null)
        {
            return;
        }

        // Hat-tip: https://stackoverflow.com/a/36684660/44540 🙇‍♂️
        Entry(current).CurrentValues.SetValues(item);

        await SaveChangesAsync();
    }

    public async ValueTask DeleteItem(Guid id)
    {
        var item = await Items.FindAsync(id);
        if (item is null)
        {
            return;
        }

        Items.Remove(item);

        await SaveChangesAsync();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Item>()
            .HasKey(e => e.Id);

        // modelBuilder.Entity<Item>()
        //     .HasIndex(e => e.Name);
        //
        // modelBuilder.Entity<Item>()
        //     .HasIndex(e => e.Status);

        var maxLengthOfStatus = Enum.GetNames(typeof(ItemStatus))
            .Select(e => e.Length)
            .Max();

        modelBuilder.Entity<Item>()
            .Property(e => e.Status)
            .HasConversion<string>()
            .HasMaxLength(maxLengthOfStatus);
    }
}
