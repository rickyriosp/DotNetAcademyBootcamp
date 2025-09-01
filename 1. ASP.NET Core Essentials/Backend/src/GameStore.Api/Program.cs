using GameStore.Api.Data;
using GameStore.Api.Features.Games;
using GameStore.Api.Features.Genres;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Register Services
builder.Services.AddTransient<GameDataLogger>();
builder.Services.AddSingleton<GameStoreData>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGames();
app.MapGenres();

app.Run();