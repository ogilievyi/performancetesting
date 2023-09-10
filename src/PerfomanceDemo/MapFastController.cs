using Map;
using Map.Entity;
using Map.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace PerformanceDemo;

[ApiController]
[Route("[controller]")]

public class MapFastController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly CustomLogger _logger;

    public MapFastController(IConfiguration configuration)
    {
        _configuration = configuration;
        _logger = new CustomLogger(configuration, typeof(Program));
    }

    [HttpGet("get_address")]
    public object GetAddress([FromQuery] string address)
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
            _logger.ExternalLogger.Error($"{DateTime.UtcNow}: Get Address fast with param \"{address}\" failed", e);
            throw;
        }
        finally
        {
            _logger.ExternalLogger.Info($"{DateTime.UtcNow}: Get Address fast End with param \"{address}\"");
        }
    }


    [HttpGet("get_addresses")]
    public object GetAddresses([FromQuery] string address)
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
            _logger.ExternalLogger.Error($"{DateTime.UtcNow}: Get Addresses fast with param \"{address}\" failed", e);
            throw;
        }
        finally
        {
            _logger.ExternalLogger.Info($"{DateTime.UtcNow}: Get Addresses fast End with param \"{address}\"");
        }
    }

    [HttpGet("set_statistic")]
    public object SetStatistic([FromQuery] string address, string userId)
    {
        var statisticRepository = new StatisticRepository(_configuration);
        statisticRepository.Add(userId, address);
        return Ok();
    }
}