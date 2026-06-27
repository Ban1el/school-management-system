using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace API.Models.Data;

public class AppDbContextDapper
{
    private readonly IConfiguration _configuration;
    private readonly string _connectionString1;

    public AppDbContextDapper(IConfiguration configuration)
    {
        _configuration = configuration;
        _connectionString1 = configuration.GetConnectionString("SMSDatabase") ?? "";
    }

    public IDbConnection CreateConnection(int _connection = 0)
    {
        switch (_connection)
        {
            case 0:
                return new SqlConnection(_connectionString1);
            default:
                throw new ArgumentException("Invalid connection identifier. Use 0 for SMS.");
        }
    }
}