namespace DemoApi;

internal sealed record Item
{
    public int Id { get; init; }
    public string? Name { get; init; }
    public bool Status { get; init; }
}