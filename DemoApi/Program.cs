var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
var logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();
builder.Logging.AddSerilog(logger);

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.RegisterInMemoryDbModule();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseSwagger();
app.UseSwaggerUI();

// Redirect root to swagger
app.MapGet("/", () => Results.Redirect("/swagger"))
    .ExcludeFromDescription();

app.MapInMemoryDbEndpoints();

// TODO Create polly module to demo API resilience
// TODO Create kube module to demo kube integration

app.Run();
