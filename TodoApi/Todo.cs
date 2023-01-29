namespace TodoApi;

internal sealed record Todo
{
    public int Id { get; init; }
    public string? Name { get; init; }
    public bool IsComplete { get; init; }
}

internal class TodoDb : DbContext
{
    public TodoDb(DbContextOptions<TodoDb> options)
        : base(options) { }

    public DbSet<Todo> Todos => Set<Todo>();

    internal static async Task<IResult> GetAllTodos(TodoDb db) =>
        TypedResults.Ok(await db.Todos.ToArrayAsync());

    internal static async Task<IResult> GetCompleteTodos(TodoDb db) =>
        TypedResults.Ok(await db.Todos.Where(t => t.IsComplete).ToListAsync());

    internal static async Task<IResult> GetTodo(int id, TodoDb db) =>
        await db.Todos.FindAsync(id) is { } todo
            ? TypedResults.Ok(todo)
            : TypedResults.NotFound();

    internal static async Task<IResult> CreateTodo(Todo todo, TodoDb db)
    {
        db.Todos.Add(todo);

        await db.SaveChangesAsync();

        return TypedResults.Created($"/todoitems/{todo.Id}", todo);
    }

    internal static async Task<IResult> UpdateTodo(int id, Todo inputTodo, TodoDb db)
    {
        if (await db.Todos.FindAsync(id) is not { } todo)
        {
            return TypedResults.NotFound();
        }

        todo = todo with { Name = inputTodo.Name, IsComplete = inputTodo.IsComplete };

        await db.SaveChangesAsync();

        return TypedResults.NoContent();
    }

    internal static async Task<IResult> DeleteTodo(int id, TodoDb db)
    {
        if (await db.Todos.FindAsync(id) is not { } todo)
        {
            return TypedResults.NotFound();
        }

        db.Todos.Remove(todo);

        await db.SaveChangesAsync();

        return TypedResults.Ok(todo);
    }
}