using Map;
using Map.Entity;
using Map.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace PerformanceDemo;

[ApiController]
[Route("[controller]")]

public class MapController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly CustomLogger _logger;

    public MapController(IConfiguration configuration)
    {
        _configuration = configuration;
        _logger = new CustomLogger(configuration, typeof(Program));
    }

    [HttpGet("get_address")]
    public object GetAddress([FromQuery] string address, string userId)
    {
        _logger.WriteInfo($"Get Address Start with param \"{address}\"");
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
            _logger.WriteError(e, $"Get Address with param \"{address}\" failed");
            throw;
        }
        finally
        {
            _logger.WriteInfo($"Get Address End with param \"{address}\"");
        }
    }

    [HttpGet("get_addresses")]
    public object GetAddresses([FromQuery] string address, string userId)
    {
        _logger.WriteInfo($"Get Address Start with param \"{address}\"");
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
            _logger.WriteError(e, $"Get Addresses with param \"{address}\" failed");
            throw;
        }
        finally
        {
            _logger.WriteInfo($"Get Addresses End with param \"{address}\"");
        }
    }
}