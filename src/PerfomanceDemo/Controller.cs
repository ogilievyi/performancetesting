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

    private readonly IConfiguration _configuration;
    private readonly CustomLogger _logger;

    public DemoController(IConfiguration configuration)
    {
        _configuration = configuration;
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

    [HttpGet("get_address")]
    public object GetAddress([FromQuery] string address, string userId)
    {
        _logger.WriteInfo($"{DateTime.UtcNow}: Get Address Start with param \"{address}\"");
        try
        {
            var statisticRepository = new StatisticRepository(_configuration);
            statisticRepository.Add(userId, address);
            var mapService = new MapService(_configuration);
            var result = mapService.GetAddressByPart(address);
            return result ?? new Address();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            _logger.WriteError(e, $"{DateTime.UtcNow}: Get Address with param \"{address}\" failed");
            throw;
        }
        finally
        {
            _logger.WriteInfo($"{DateTime.UtcNow}: Get Address End with param \"{address}\"");
        }
    }

    [HttpGet("get_address_fast")]
    public object GetAddressFast([FromQuery] string address)
    {
        _logger.ExternalLogger.Info($"{DateTime.UtcNow}: Get Address fast Start with param \"{address}\"");
        try
        {
            var mapService = new MapServiceFast(_configuration);
            var result = mapService.GetAddressByPart(address);
            return result ?? new Address();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            _logger.ExternalLogger.Error($"{DateTime.UtcNow}: Get Address fast with param \"{address}\" failed", e);
            throw;
        }
        finally
        {
            _logger.ExternalLogger.Info($"{DateTime.UtcNow}: Get Address fast End with param \"{address}\"");
        }
    }

    [HttpGet("get_addresses")]
    public object GetAddresses([FromQuery] string address, string userId)
    {
        _logger.WriteInfo($"{DateTime.UtcNow}: Get Address Start with param \"{address}\"");
        try
        {
            var statisticRepository = new StatisticRepository(_configuration);
            statisticRepository.Add(userId, address);
            var mapService = new MapService(_configuration);
            var result = mapService.GetAddressesByPart(address);
            return (object)result ?? new[] { new Address() };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            _logger.WriteError(e, $"{DateTime.UtcNow}: Get Addresses with param \"{address}\" failed");
            throw;
        }
        finally
        {
            _logger.WriteInfo($"{DateTime.UtcNow}: Get Addresses End with param \"{address}\"");
        }
    }

    [HttpGet("get_addresses_fast")]
    public object GetAddressesFast([FromQuery] string address)
    {
        _logger.ExternalLogger.Info($"{DateTime.UtcNow}: Get Addresses fast Start with param \"{address}\"");
        try
        {
            var mapService = new MapServiceFast(_configuration);
            var result = mapService.GetAddressesByPart(address);
            return (object)result ?? new[] { new Address() };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            _logger.WriteError(e, $"{DateTime.UtcNow}: Get Addresses fast with param \"{address}\" failed");
            throw;
        }
        finally
        {
            _logger.WriteInfo($"{DateTime.UtcNow}: Get Addresses fast End with param \"{address}\"");
        }
    }
}