var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddConfiguration();

builder.Configuration.AddEnvironmentVariables("DemoHttp_");

builder.Services.AddDatabasesInAssembly();
builder.Services.AddHttpsRedirection(_ => { }); // Defaults to HTTPS_PORT environment variable
builder.Services.ConfigureJsonOptions();

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDatabaseDeveloperPageExceptionFilter();
}

var app = builder.Build();

app.UseEndpointsInAssembly();
app.UseHttpsRedirection();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

var version = Assembly
    .GetExecutingAssembly()!
    .GetCustomAttribute<AssemblyFileVersionAttribute>()?
    .Version;
app.Logger.LogInformation("Running demo http {Version}", version);

app.Run();
