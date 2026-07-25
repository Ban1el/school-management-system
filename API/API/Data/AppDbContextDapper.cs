using API.Options;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System;
using System.Data;

namespace API.Models.Data;

public class AppDbContextDapper
{
    private readonly IConfiguration _configuration;
    private readonly DBConnectionOptions _dbConnectionOptions;

    public AppDbContextDapper(IConfiguration configuration, IOptions<DBConnectionOptions> _options)
    {
        _configuration = configuration;
        _dbConnectionOptions = _options.Value;
    }

    public IDbConnection CreateConnection(int _connection = 0)
    {
        switch (_connection)
        {
            case 0:
                return new SqlConnection(_dbConnectionOptions.SMSDatabase);
            default:
                throw new ArgumentException("Invalid connection identifier. Use 0 for SMS.");
        }
    }
}