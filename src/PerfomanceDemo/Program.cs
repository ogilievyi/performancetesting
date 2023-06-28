using App.Metrics;
using App.Metrics.AspNetCore;
using App.Metrics.Filtering;
using App.Metrics.Formatters.Prometheus;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using PerformanceDemo;

var metricsContextName = "demo_webapp";
var filter = new MetricsFilter().WhereContext(metricsContextName);
var metrics = new MetricsBuilder()
    .Filter.With(filter)
    .OutputMetrics.AsPrometheusPlainText()
    .OutputMetrics.AsPrometheusProtobuf()
    .Configuration.Configure(new MetricsOptions
    {
        DefaultContextLabel = metricsContextName
    })
    .Build();

var builder = WebHost.CreateDefaultBuilder(args)
    .ConfigureMetrics(metrics)
    .UseMetrics(options =>
    {
        options.EndpointOptions = endpointsOptions =>
        {
            endpointsOptions.MetricsEndpointOutputFormatter =
                metrics.OutputMetricsFormatters.FirstOrDefault(a =>
                    a is MetricsPrometheusTextOutputFormatter);
        };
    });
builder.UseStartup<Startup>();
var app = builder.Build();
app.Run();
