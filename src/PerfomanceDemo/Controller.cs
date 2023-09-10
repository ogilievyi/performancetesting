using System.Diagnostics;
using Map;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace PerformanceDemo;

[ApiController]
public class DemoController : ControllerBase
{
    private static int _timeout;
    private static int _concurrentOkTimeoutCount;

    private readonly CustomLogger _logger;

    public DemoController(IConfiguration configuration)
    {
        _logger = new CustomLogger(configuration, typeof(Program));
    }

    [HttpGet("ok")]
    public object GetOk()
    {
        _logger.WriteInfo($"{DateTime.UtcNow}: simple ok!");
        return base.Ok(new { result = "Ok" });
    }

    [HttpGet("ok_timeout")]
    public object GetOkTimeout()
    {
        Interlocked.Increment(ref _concurrentOkTimeoutCount);
        var s = new Stopwatch();
        s.Start();
        _logger.WriteInfo($"{DateTime.UtcNow}: timeout ok started. Thread: {_concurrentOkTimeoutCount}");
        Thread.Sleep(TimeSpan.FromSeconds(_timeout));
        try
        {
            return base.Ok(new { result = "Ok" });
        }
        finally
        {
            Interlocked.Decrement(ref _concurrentOkTimeoutCount);
            _logger.WriteInfo(
                $"{DateTime.UtcNow}: timeout ok finished. Elapsed: {s.Elapsed}, Thread: {_concurrentOkTimeoutCount}");
        }
    }

    [HttpGet("set_timeout")]
    public void SetTimeout([FromQuery] int timeout)
    {
        _logger.WriteInfo($"{DateTime.UtcNow}: set_timeout! Value: {timeout}");
        _timeout = timeout;
    }
}