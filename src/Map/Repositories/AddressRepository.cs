using System.Data;
using System.Data.SqlClient;
using System.Text;
using Map.Entity;
using Microsoft.Extensions.Configuration;

namespace Map.Repositories;

public class AddressRepository : BaseRepository<Address>
{
    public AddressRepository(IConfiguration configuration) : base(configuration)
    {
    }

    protected override List<Address> Read(SqlDataReader reader)
    {
        var list = new List<Address>();
        while (reader.Read())
        {
            var address = new Address
            {
                Region = reader[nameof(Address.Region)] as string ?? string.Empty,
                District = reader[nameof(Address.District)] as string ?? string.Empty,
                City = reader[nameof(Address.City)] as string ?? string.Empty,
                CityType = reader[nameof(Address.CityType)] as string ?? string.Empty,
                Street = reader[nameof(Address.Street)] as string ?? string.Empty,
                BuildNumber = reader[nameof(Address.BuildNumber)] as string ?? string.Empty,
                Index = reader[nameof(Address.Index)] as string ?? string.Empty
            };
            list.Add(address);
        }

        return list;
    }

    public virtual string GetSql(string part)
    {
        var sql = "select * from Address where " +
                  $"Region like N'%{part}%' " +
                  $"or District like N'%{part}%' " +
                  $"or City like N'%{part}%' " +
                  $"or CityType like N'%{part}%' " +
                  $"or Street like N'%{part}%' " +
                  $"or BuildNumber like N'%{part}%' " +
                  $"or [Index] like N'%{part}%' " +
                  " ";
        return sql;
    }

    public virtual Address GetAddress(string part)
    {
        var sql = GetSql(part);
        return SelectSql(sql).FirstOrDefault();
    }

    public virtual List<Address> GetAddresses(string part)
    {
        var sql = GetSql(part);
        return SelectSql(sql);
    }
}

public class AddressRepositoryFast : AddressRepository
{
    private SqlConnection _connection;

    public AddressRepositoryFast(IConfiguration configuration) : base(configuration)
    {
    }

    public override string GetSql(string part)
    {
        var sql = $"select  * from Address2 (nolock) where id in ({part})";
        return sql;
    }

    private SqlConnection GetDbConnection()
    {
        if (_connection is { State: ConnectionState.Open }) return _connection;
        var connectionString = Configuration.GetConnectionString("db");
        var connection = new SqlConnection(connectionString);
        connection.Open();
        _connection = connection;
        return connection;
    }

    public override Address GetAddress(string part)
    {
        var connection = GetDbConnection();
        using var command = connection.CreateCommand();
        command.CommandText = $"select top 2000 id from AddressIndex (nolock) where address like N'%{part}%' ";
        using var reader = command.ExecuteReader();
        var sb = new StringBuilder();
        while (reader.Read())
        {
            var id = reader["Id"];
            sb.Append($"{id},");
        }

        sb.Remove(sb.Length - 1, 1);
        return base.GetAddress(sb.ToString());
    }

    protected override List<Address> SelectSql(string sql)
    {
        var connection = GetDbConnection();
        using var command = connection.CreateCommand();
        command.CommandText = sql;
        using var reader = command.ExecuteReader();
        var list = Read(reader);
        return list;
    }
}