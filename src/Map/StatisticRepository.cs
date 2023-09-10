﻿using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Map;

public class StatisticRepository : BaseRepository<Statistic>
{
    public StatisticRepository(IConfiguration configuration) : base(configuration)
    {
    }

    public void Add(string userId, string searchText)
    {
        var sql = $"insert into Statistic (user, searchText, count) values ('{userId}', '{searchText}', 1)";
        InsertSql(sql);
    }

    protected override List<Statistic> Read(SqlDataReader reader)
    {
        var list = new List<Statistic>();
        while (reader.Read())
        {
            var address = new Statistic
            {
                User = reader[nameof(Statistic.User)] as string ?? string.Empty,
                SearchText = reader[nameof(Statistic.SearchText)] as string ?? string.Empty,
                Count = reader[nameof(Statistic.Count)] as string ?? string.Empty
            };
            list.Add(address);
        }

        return list;
    }
}