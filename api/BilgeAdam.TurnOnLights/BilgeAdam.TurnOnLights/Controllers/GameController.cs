using BilgeAdam.TurnOnLights.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using StackExchange.Redis;

namespace BilgeAdam.TurnOnLights.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GameController : ControllerBase
    {
        private readonly IDatabase redis;
        private readonly IHubContext<GameHub, IGameHub> hubContext;

        public GameController(IConnectionMultiplexer connection, IHubContext<GameHub, IGameHub> hubContext)
        {
            this.redis = connection.GetDatabase();
            this.hubContext = hubContext;
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
        public async Task<UserDto> Register([FromBody] string userName)
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
            await hubContext.Clients.All.UserJoined(user.Id.ToString(), user.Name);
            return user;
        }
    }
}