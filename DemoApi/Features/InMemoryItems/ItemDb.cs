namespace DemoApi.Features.InMemoryItems;

internal sealed class ItemDb : DbContext
{
    public bool BooleanEnvironmentVariable { get; }

    public ItemDb(DbContextOptions<ItemDb> options, IOptions<DemoApiOptions> demoApiOptions)
        : base(options)
    {
        BooleanEnvironmentVariable = demoApiOptions.Value.BooleanEnvironmentVariable;
    }

    public DbSet<Item> Items => Set<Item>();
}
