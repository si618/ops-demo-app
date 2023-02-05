﻿namespace DemoApi.Features.InMemoryItems;

public sealed record Item
{
    public Guid Id { get; init; }

    public string? Name { get; init; }

    public ItemStatus Status { get; init; }
}