using BilgeAdam.TurnOnLights.Models;
using Microsoft.AspNetCore.SignalR;
using StackExchange.Redis;

namespace BilgeAdam.TurnOnLights
{
    public class GameHub : Hub<IGameHub>
    {
        private readonly IDatabase redis;
        public GameHub(IConnectionMultiplexer connection)
        {
            this.redis = connection.GetDatabase();
        }

        public Task NewUserJoined(string id, string name)
        {
            redis.StringSet(Context.ConnectionId, id);
            var fields = new HashEntry[2]
            {
                new HashEntry(nameof(UserDto.Name), name),
                new HashEntry(nameof(UserDto.ConnectionId), Context.ConnectionId),
            };
            redis.HashSet($"User:{id}", fields);
            redis.ListLeftPush("AllUsers", id.ToString());
            Clients.All.UserJoined(id.ToString(), name);
            return Task.CompletedTask;
        }

        public async Task OnWindowTaken(int id)
        {
            await Clients.Others.WindowTaken(id);
        }

        public async Task OnWindowStatusChanged(int id)
        {
            await Clients.Others.WindowStatusChanged(id);
        }

        public async Task SendMessage(string id, string message)
        {
            var connectionId = redis.HashGet($"User:{id}", nameof(UserDto.ConnectionId));
            if (!connectionId.HasValue)
            {
                return;
            }
            await Clients.Client(connectionId).MessageSent(message);
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            var id = redis.StringGet(Context.ConnectionId);
            if (id.HasValue)
            {
                redis.KeyDelete(Context.ConnectionId);
                redis.KeyDelete($"User:{id}");
                redis.ListRemove("AllUsers", id); 
                Clients.All.UserLeft(id.ToString());
            }
            return base.OnDisconnectedAsync(exception);
        }
    }

    public interface IGameHub
    {
        Task UserJoined(string id, string name);
        Task WindowTaken(int id);
        Task WindowStatusChanged(int id);
        Task UserLeft(string id);
        Task MessageSent(string message);
    }
}
