namespace DemoApi.Infrastructure;

public static class LoggingBuilderExtensions
{
    public static void AddConfiguration(this ILoggingBuilder logging)
    {
        logging.ClearProviders();

        var logger = new LoggerConfiguration()
            .Enrich.WithExceptionDetails(new DestructuringOptionsBuilder()
                .WithDefaultDestructurers()
                .WithDestructurers(new[] { new ApiExceptionDestructurer() }))
            .WriteTo.Console()
            // TODO Deploy and configure ELK stack in kube, maybe using OpenTelemetry agent?
            //.WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://localhost:9200")))
            .CreateLogger();

        logging.AddSerilog(logger);
    }
}
