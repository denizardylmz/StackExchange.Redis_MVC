using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace RedisWork.Services;

public class RedisServer
{

    private ConnectionMultiplexer _connectionMultiplexer;
    private IDatabase _database;
    private string configurationString;
    private int _currentDbId = 0;
    
    public RedisServer(IConfiguration configuration)
    {
        CreateRedisConnectionString(configuration);
        
        _connectionMultiplexer = ConnectionMultiplexer.Connect(configurationString);
        _database = _connectionMultiplexer.GetDatabase(_currentDbId);
    }

    public IDatabase Database => _database;

    public void FlushDatabase()
    {
        _connectionMultiplexer.GetServer(configurationString).FlushDatabase(_currentDbId);
    }

    private void  CreateRedisConnectionString(IConfiguration configuration)
    {
        string host = configuration.GetSection("RedisConfiguration:Host").Value;
        string port = configuration.GetSection("RedisConfiguration:Port").Value;

        configurationString = $"{host}:{port}";
    }
    
    
}