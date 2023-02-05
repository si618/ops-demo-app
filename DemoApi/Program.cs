var builder = WebApplication.CreateBuilder(args);

builder.AddLogging();
builder.Configuration.AddEnvironmentVariables("DemoApi_");
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddEndpointsInAssembly();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseEndpointsInAssembly();

// TODO Create polly module to demo API resilience
// TODO Create kube module to demo kube integration & resilience

app.Run();
