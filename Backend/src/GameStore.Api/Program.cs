using GameStore.Api.Data;
using GameStore.Api.Features.Games;
using GameStore.Api.Features.Genres;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

var connString = builder.Configuration.GetConnectionString("GameStore");

/* What service lifetime to use for a DbContext
 - DbContext is design to be used as a single Unit of Work
 - DbContext created -> entity changes tracked -> save changes -> dispose
 - DB connections are expensive
 - DbContext is not thread safe
 - Increased memory usage due to change tracking

 USE: Scoped service lifetime
 - Aligning the context lifetime to the lifetime of the request
 - There is only one thread executing each client request at a given time
 - Ensure each request gets a separate DbContext instance
 */

// builder.Services.AddDbContext<GameStoreContext>(options => options.UseSqlite(connString));
builder.Services.AddSqlite<GameStoreContext>(connString);

// Register Services
builder.Services.AddTransient<GameDataLogger>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGames();
app.MapGenres();
await app.InitializeDbAsync();

app.Run();