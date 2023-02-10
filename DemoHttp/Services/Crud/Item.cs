namespace DemoHttp.Services.Crud;

public sealed record Item
{
    public Guid Id { get; init; }

    public string? Name { get; init; }

    public ItemStatus Status { get; init; }
}
