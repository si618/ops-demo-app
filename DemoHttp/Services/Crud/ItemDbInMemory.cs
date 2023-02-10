namespace DemoHttp.Services.Crud;

internal class ItemDbInMemory : ItemDbBase
{
    public IServiceCollection DefineServices(IServiceCollection services)
    {
        services.AddDbContext<ItemDbInMemory>();
        services.AddScoped<IItemDb, ItemDbInMemory>();

        return services;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase("Items");
    }
}
