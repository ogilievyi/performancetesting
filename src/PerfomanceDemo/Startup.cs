using log4net;
using log4net.Config;
using Map;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace PerformanceDemo;

public class Startup
{
    public Startup(IWebHostEnvironment env)
    {
        HostingEnvironment = env;
        Configuration = new ConfigurationBuilder()
            .SetBasePath(HostingEnvironment.ContentRootPath)
            .AddJsonFile("appsettings.json", true, true)
            .AddEnvironmentVariables()
            .Build();
    }

    public IConfiguration Configuration { get; set; }

    public IWebHostEnvironment HostingEnvironment { get; }

    protected void InitLog4NetPath()
    {
        var repository = LogManager.CreateRepository("default");
        var fileName = Configuration["Log4NetPath"];
        if (string.IsNullOrEmpty(fileName))
            fileName = "log4net.config";
        var configFile = new FileInfo(fileName);
        XmlConfigurator.Configure(repository, configFile);
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton(Configuration);
        services.AddSingleton(new MapServiceFast(Configuration));
        services.Configure<KestrelServerOptions>(o => { o.AllowSynchronousIO = true; });
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddLogging(c =>
        {
            c.ClearProviders();
            c.AddLog4Net();
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        InitLog4NetPath();
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            options.RoutePrefix = string.Empty;
        });
        app.UseRouting();
        app.UseHttpsRedirection();
        app.UseEndpoints(e => e.MapControllers());
    }
}