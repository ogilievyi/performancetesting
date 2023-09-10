using Microsoft.Extensions.Configuration;

namespace Map;

public class MapService
{
    private AddressRepository _addressRepository;

    protected virtual AddressRepository Repository
    {
        get => _addressRepository;
        set => _addressRepository = value;
    }

    public MapService(IConfiguration configuration)
    {
        _addressRepository = new AddressRepository(configuration);
    }

    public Address GetAddressByPart(string part)
    {
        return _addressRepository.GetAddress(part);
    }

    public List<Address> GetAddressesByPart(string part)
    {
        return _addressRepository.GetAddresses(part);
    }
}

public sealed class MapServiceFast : MapService
{
    public MapServiceFast(IConfiguration configuration) : base(configuration)
    {
        Repository = new AddressRepositoryFast(configuration);
    }
}