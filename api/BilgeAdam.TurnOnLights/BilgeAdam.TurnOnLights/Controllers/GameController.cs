using BilgeAdam.TurnOnLights.Models;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace BilgeAdam.TurnOnLights.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GameController : ControllerBase
    {
        private readonly IDatabase redis;
        public GameController(IConnectionMultiplexer connection)
        {
            this.redis = connection.GetDatabase();
        }
        [HttpGet("users")]
        public IEnumerable<UserDto> Users()
        {
            var userIds = redis.ListRange("AllUsers", 0);
            foreach (var userId in userIds)
            {
                var entries = redis.HashGetAll($"User:{userId}");
                yield return new UserDto
                {
                    Id = Guid.Parse(userId),
                    Name = entries[0].Value
                };
            }
        }

        [HttpPost("register")]
        public UserDto Register([FromBody] string userName)
        {
            var user = new UserDto
            {
                Id = Guid.NewGuid(),
                Name = userName
            };
            var fields = new HashEntry[1]
            {
                new HashEntry(nameof(UserDto.Name), user.Name)
            };
            redis.HashSet($"User:{user.Id}", fields);

            redis.ListLeftPush("AllUsers", user.Id.ToString());

            return user;
        }
    }
}