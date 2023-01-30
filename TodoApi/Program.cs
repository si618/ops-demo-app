var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TodoDb>(opt => opt.UseInMemoryDatabase("TodoList"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/", () => Results.Redirect("/swagger"))
    .ExcludeFromDescription();

var todo = app.MapGroup("/todo")
    .WithTags("Todo");

todo.MapGet("/", TodoDb.GetAllTodos)
    .Produces<Todo>();

todo.MapGet("/complete", TodoDb.GetCompleteTodos)
    .Produces<Todo>();

todo.MapGet("/{id:int}", TodoDb.GetTodo)
    .Produces<Todo>()
    .Produces(StatusCodes.Status404NotFound);

todo.MapPost("/", TodoDb.CreateTodo)
    .Produces(StatusCodes.Status201Created);

todo.MapPut("/{id:int}", TodoDb.UpdateTodo)
    .Produces(StatusCodes.Status204NoContent)
    .Produces(StatusCodes.Status404NotFound)
    .WithOpenApi();

todo.MapDelete("/{id:int}", TodoDb.DeleteTodo)
    .Produces(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status404NotFound);

app.Run();
