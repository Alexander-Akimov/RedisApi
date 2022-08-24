using Microsoft.AspNetCore.Mvc;
using RedisApi.Data;
using RedisApi.Models;

namespace RedisApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PlatformsController : ControllerBase
{
    private readonly IPlatformRepo _platformRepo;

    public PlatformsController(IPlatformRepo platformRepo)
    {
        _platformRepo = platformRepo;
    }

    [HttpGet("{id}", Name = "GetPlatformById")]
    public ActionResult<Platform> GetPlatformById(string id)
    {
        var platform = _platformRepo.GetPlatformById(id);
        if (platform != null)
            return Ok(platform);
        return NotFound();
    }

    [HttpPost]
    public ActionResult<Platform> CreatePlatform(Platform platform)
    {
        _platformRepo.CreatePlatform(platform);

        return CreatedAtRoute(nameof(GetPlatformById), new {Id = platform.Id}, platform);
    }
    
    [HttpGet(Name = "GetAllPlatforms")]
    public ActionResult<IEnumerable<Platform>> GetAllPlatforms()
    {
        var platforms = _platformRepo.GetAllPlatforms();
        if (platforms?.Count() > 0)
            return Ok(platforms);
        return NotFound();
    }
}