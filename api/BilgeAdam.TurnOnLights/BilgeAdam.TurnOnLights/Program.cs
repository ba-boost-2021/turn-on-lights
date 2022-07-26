using BilgeAdam.TurnOnLights;
using BilgeAdam.TurnOnLights.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddSignalR();
builder.Services.AddCors(options =>
{
    options.AddPolicy("All", config =>
    {
        config.WithOrigins("http://localhost:5173", "https://game.peerque.com").AllowAnyHeader().AllowAnyMethod().AllowCredentials();
    });
});

builder.Services.AddRedis();

var app = builder.Build();
app.UseHttpsRedirection();
app.UseCors("All");
app.UseAuthorization();
app.MapControllers();
app.MapHub<GameHub>("/game");
app.Run();
