using System.Diagnostics;
using log4net;
using Microsoft.AspNetCore.Mvc;

namespace PerformanceDemo;

[ApiController]
public class DemoController : ControllerBase
{
    private static readonly Lazy<ILog> Log4NetLazy = new(LogManager.GetLogger(typeof(Program)));
    private static int _timeout;
    private static int _concurrentOkTimeoutCount;
    private static ILog Logger => Log4NetLazy.Value;

    [HttpGet("ok")]
    public object GetOk()
    {
        Logger.Info($"{DateTime.UtcNow}: simple ok!");
        return base.Ok(new { result = "Ok" });
    }

    [HttpGet("ok_timeout")]
    public object GetOkTimeout()
    {
        Interlocked.Increment(ref _concurrentOkTimeoutCount);
        var s = new Stopwatch();
        s.Start();
        Logger.Info($"{DateTime.UtcNow}: timeout ok started. Thread: {_concurrentOkTimeoutCount}");
        Thread.Sleep(TimeSpan.FromSeconds(_timeout));
        try
        {
            return base.Ok(new { result = "Ok" });
        }
        finally
        {
            Interlocked.Decrement(ref _concurrentOkTimeoutCount);
            Logger.Info(
                $"{DateTime.UtcNow}: timeout ok finished. Elapsed: {s.Elapsed}, Thread: {_concurrentOkTimeoutCount}");
        }
    }

    [HttpGet("set_timeout")]
    public void SetTimeout([FromQuery] int timeout)
    {
        Logger.Info($"{DateTime.UtcNow}: set_timeout! Value: {timeout}");
        _timeout = timeout;
    }
}