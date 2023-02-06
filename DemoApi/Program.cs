var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddConfiguration();

builder.Configuration.AddEnvironmentVariables("DemoApi_");
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Use HTTPS_PORT environment variable
builder.Services.AddHttpsRedirection(_ => { });
builder.Services.AddEndpointsInAssembly();

var app = builder.Build();

app.UseDeveloperExceptionPage();
app.UseHttpsRedirection();
app.UseEndpointsInAssembly();

app.Run();
