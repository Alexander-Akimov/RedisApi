using System.Text.Json;
using RedisApi.Models;
using StackExchange.Redis;

namespace RedisApi.Data;

public class RedisPlatformRepo : IPlatformRepo
{
    private readonly IConnectionMultiplexer _redis;

    public RedisPlatformRepo(IConnectionMultiplexer redis)
    {
        _redis = redis;
    }

    public void CreatePlatform(Platform plat)
    {
        ArgumentNullException.ThrowIfNull(plat);

        var db = _redis.GetDatabase();

        var serialPlat = JsonSerializer.Serialize(plat);

        // db.StringSet(plat.Id, serialPlat);
        // db.SetAdd("PlatformSet", serialPlat);
        
        db.HashSet("hashplatform", new HashEntry[]
        {
            new HashEntry(plat.Id, serialPlat)
        });
    }

    public Platform? GetPlatformById(string id)
    {
        var db = _redis.GetDatabase();
        // var plat = db.StringGet(id);
        var plat = db.HashGet("hashplatform", id);
        if (string.IsNullOrEmpty(plat)) return null;
        return JsonSerializer.Deserialize<Platform>(plat);
    }

    public IEnumerable<Platform?>? GetAllPlatforms()
    {
        var db = _redis.GetDatabase();
        //var completeSet = db.SetMembers("PlatformSet");
        // if (completeSet.Length <= 0) return null;
        // var obj = Array.ConvertAll(completeSet,
        // val => JsonSerializer.Deserialize<Platform>(val)).ToList();

        var completeHash = db.HashGetAll("hashplatform");
        if (completeHash.Length <= 0) return null;
        var obj = Array.ConvertAll(completeHash,
            val => JsonSerializer.Deserialize<Platform>(val.Value)).ToList();
        return obj;
    }
}