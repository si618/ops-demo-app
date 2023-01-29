var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TodoDb>(opt => opt.UseInMemoryDatabase("TodoList"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseSwagger();
app.UseSwaggerUI();

var todo = app.MapGroup("/todo");

todo.MapGet("/", TodoDb.GetAllTodos)
    .Produces<Todo>()
    .WithOpenApi();

todo.MapGet("/complete", TodoDb.GetCompleteTodos)
    .Produces<Todo>()
    .WithOpenApi();

todo.MapGet("/{id:int}", TodoDb.GetTodo)
    .Produces<Todo>()
    .Produces(StatusCodes.Status404NotFound)
    .WithOpenApi();

todo.MapPost("/", TodoDb.CreateTodo)
    .Produces(StatusCodes.Status201Created)
    .WithOpenApi();

todo.MapPut("/{id:int}", TodoDb.UpdateTodo)
    .Produces(StatusCodes.Status204NoContent)
    .Produces(StatusCodes.Status404NotFound)
    .WithOpenApi();

todo.MapDelete("/{id:int}", TodoDb.DeleteTodo)
    .Produces(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status404NotFound)
    .WithOpenApi();

app.Run(context =>
{
    context.Response.Redirect("swagger");
    return Task.CompletedTask;
});
