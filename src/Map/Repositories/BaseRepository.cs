using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Map.Repositories;

public abstract class BaseRepository<T>
{
    protected readonly IConfiguration Configuration;

    protected BaseRepository(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    protected abstract List<T> Read(SqlDataReader reader);

    protected virtual List<T> SelectSql(string sql)
    {
        var connectionString = Configuration.GetConnectionString("db");
        using var connection = new SqlConnection(connectionString);
        connection.Open();
        var transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted);
        var command = connection.CreateCommand();
        command.CommandText = sql;
        command.Transaction = transaction;
        using var reader = command.ExecuteReader();
        var list = Read(reader);

        reader.Close();
        reader.Dispose();
        transaction.Commit();
        transaction.Dispose();
        command.Dispose();
        connection.Close();
        connection.Dispose();
        return list;
    }

    protected void InsertSql(string sql)
    {
        var connectionString = Configuration.GetConnectionString("db");
        using var connection = new SqlConnection(connectionString);
        connection.Open();
        var transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted);
        var command = connection.CreateCommand();
        command.CommandText = sql;
        command.Transaction = transaction;
        command.ExecuteNonQuery();
        transaction.Commit();
        transaction.Dispose();
        command.Dispose();
        connection.Close();
        connection.Dispose();
    }
}