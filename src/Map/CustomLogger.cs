using System.Data.SqlClient;
using log4net;
using Map.Entity;
using Map.Repositories;
using Microsoft.Extensions.Configuration;

namespace Map;

public class CustomLogger : BaseRepository<Log>
{
    public CustomLogger(IConfiguration configuration, Type loggerBaseType) : base(configuration)
    {
        ExternalLogger = new Lazy<ILog>(LogManager.GetLogger(loggerBaseType)).Value;
    }

    public ILog ExternalLogger { get; }

    private void Write(string logLevel, string message, string exception)
    {
        try
        {
            var sql =
                $"insert into Log ([Date], Level, Message, Exception) values ('{DateTime.UtcNow}', N'{logLevel}', N'{message}', N'{exception}')";
            InsertSql(sql);
        }
        catch (Exception e)
        {
            ExternalLogger.Error("Db logger does not work", e);
        }
    }

    public void WriteInfo(string message)
    {
        Write("INFO", message, string.Empty);
        ExternalLogger.Info(message);
    }

    public void WriteError(Exception e, string message)
    {
        Write("ERROR", message, e.ToString());
        ExternalLogger.Error(message, e);
    }

    protected override List<Log> Read(SqlDataReader reader)
    {
        var list = new List<Log>();
        while (reader.Read())
        {
            var address = new Log
            {
                Message = reader[nameof(Log.Message)] as string ?? string.Empty,
                Exception = reader[nameof(Log.Exception)] as string ?? string.Empty
            };
            list.Add(address);
        }

        return list;
    }
}