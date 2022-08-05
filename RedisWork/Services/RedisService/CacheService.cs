using Newtonsoft.Json;

namespace RedisWork.Services;

public class CacheService : ICacheService
{
    private RedisServer _redisServer;

    public CacheService(RedisServer server)
    {
        _redisServer = server;
    }
    
    public T Get<T>(string key)
    {
        if (Any(key))
        {
            string jsonData = _redisServer.Database.StringGet(key);
            return JsonConvert.DeserializeObject<T>(jsonData);
        }

        return default;
    }

    public void Add(string key, object data)
    {
        string jsonData = JsonConvert.SerializeObject(data);
        _redisServer.Database.StringSet(key, jsonData);
    }

    public void Remove(string key)
    {
        _redisServer.Database.KeyDelete(key);
    }

    public void Clear()
    {
        _redisServer.FlushDatabase();
    }

    public bool Any(string key)
    {
        return _redisServer.Database.KeyExists(key);
    }
}