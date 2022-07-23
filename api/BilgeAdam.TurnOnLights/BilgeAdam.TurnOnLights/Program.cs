using BilgeAdam.TurnOnLights.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy("All", config =>
    {
        config.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod();
    });
});

builder.Services.AddRedis();

var app = builder.Build();
app.UseHttpsRedirection();
app.UseCors("All");
app.UseAuthorization();
app.MapControllers();
app.Run();
