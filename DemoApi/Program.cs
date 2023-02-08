var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddConfiguration();

builder.Configuration.AddEnvironmentVariables("DemoApi_");

builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddDatabasesInAssembly();
builder.Services.AddHttpsRedirection(_ => { }); // Defaults to HTTPS_PORT environment variable
builder.Services.ConfigureJsonOptions();

var app = builder.Build();

app.UseDeveloperExceptionPage();
app.UseEndpointsInAssembly();
app.UseHttpsRedirection();

app.Run();
