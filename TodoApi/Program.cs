var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TodoDb>(opt => opt.UseInMemoryDatabase("TodoList"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

var todoItems = app.MapGroup("/todoitems");

todoItems.MapGet("/", GetAllTodos)
    .Produces<Todo>()
    .WithOpenApi();

todoItems.MapGet("/complete", GetCompleteTodos)
    .Produces<Todo>()
    .WithOpenApi();

todoItems.MapGet("/{id:int}", GetTodo)
    .Produces<Todo>()
    .Produces(StatusCodes.Status404NotFound)
    .WithOpenApi();

todoItems.MapPost("/", CreateTodo)
    .Produces(StatusCodes.Status201Created)
    .WithOpenApi();

todoItems.MapPut("/{id:int}", UpdateTodo)
    .Produces(StatusCodes.Status204NoContent)
    .Produces(StatusCodes.Status404NotFound)
    .WithOpenApi();

todoItems.MapDelete("/{id:int}", DeleteTodo)
    .Produces(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status404NotFound)
    .WithOpenApi();

app.Run();

static async Task<IResult> GetAllTodos(TodoDb db) =>
    TypedResults.Ok(await db.Todos.ToArrayAsync());

static async Task<IResult> GetCompleteTodos(TodoDb db) =>
    TypedResults.Ok(await db.Todos.Where(t => t.IsComplete).ToListAsync());

static async Task<IResult> GetTodo(int id, TodoDb db) =>
    await db.Todos.FindAsync(id) is { } todo
        ? TypedResults.Ok(todo)
        : TypedResults.NotFound();

static async Task<IResult> CreateTodo(Todo todo, TodoDb db)
{
    db.Todos.Add(todo);

    await db.SaveChangesAsync();

    return TypedResults.Created($"/todoitems/{todo.Id}", todo);
}

static async Task<IResult> UpdateTodo(int id, Todo inputTodo, TodoDb db)
{
    var todo = await db.Todos.FindAsync(id);

    if (todo is null) return TypedResults.NotFound();

    todo.Name = inputTodo.Name;
    todo.IsComplete = inputTodo.IsComplete;

    await db.SaveChangesAsync();

    return TypedResults.NoContent();
}

static async Task<IResult> DeleteTodo(int id, TodoDb db)
{
    if (await db.Todos.FindAsync(id) is not { } todo)
    {
        return TypedResults.NotFound();
    }

    db.Todos.Remove(todo);
    await db.SaveChangesAsync();
    return TypedResults.Ok(todo);
}