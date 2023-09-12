using Map.Entity;
using Map.Repositories;
using Microsoft.Extensions.Configuration;
using Polly;
using Polly.Retry;

namespace Map;

public class MapService
{
    private AddressRepository _addressRepository;

    public MapService(IConfiguration configuration)
    {
        _addressRepository = new AddressRepository(configuration);
    }

    protected virtual AddressRepository Repository
    {
        get => _addressRepository;
        set => _addressRepository = value;
    }

    public virtual Address GetAddressByPart(string part)
    {
        return _addressRepository.GetAddress(part);
    }

    public virtual List<Address> GetAddressesByPart(string part)
    {
        return _addressRepository.GetAddresses(part);
    }
}

public sealed class MapServiceFast : MapService
{
    private readonly RetryPolicy _policy;

    public MapServiceFast(IConfiguration configuration) : base(configuration)
    {
        Repository = new AddressRepositoryFast(configuration);
        _policy = Policy
            .Handle<Exception>()
            .WaitAndRetryForever(retryAttempt =>
                TimeSpan.FromMilliseconds(new Random().Next(1000, 1000 + 100 * retryAttempt)));
    }

    public override Address GetAddressByPart(string part)
    {
        return _policy.Execute(() => base.GetAddressByPart(part));
    }

    public override List<Address> GetAddressesByPart(string part)
    {
        return _policy.Execute(() => base.GetAddressesByPart(part));
    }
}