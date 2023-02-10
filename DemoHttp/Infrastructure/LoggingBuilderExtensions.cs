namespace DemoHttp.Infrastructure;

public static class LoggingBuilderExtensions
{
    public static void AddConfiguration(this ILoggingBuilder logging)
    {
        logging.ClearProviders();

        // TODO Deploy and configure Elastic Stack in Kube, maybe send logs via OpenTelemetry sink?

        var logger = new LoggerConfiguration()
            .Enrich.WithExceptionDetails(new DestructuringOptionsBuilder()
                .WithDefaultDestructurers()
                .WithDestructurers(new[] { new ApiExceptionDestructurer() }))
            .WriteTo.Console()
            .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://localhost:9200")))
            .CreateLogger();

        logging.AddSerilog(logger);
    }
}
