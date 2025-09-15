using GameStore.Api.Data;
using GameStore.Api.Features.Games;
using GameStore.Api.Features.Genres;
using GameStore.Api.Shared.ErrorHandling;

using Microsoft.AspNetCore.HttpLogging;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

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
builder.Services.AddHttpLogging(options =>
{
    options.LoggingFields = HttpLoggingFields.RequestMethod |
                            HttpLoggingFields.RequestPath |
                            HttpLoggingFields.ResponseStatusCode |
                            HttpLoggingFields.Duration;
    options.CombineLogs = true;
});

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGames();
app.MapGenres();


// Add Middleware
app.UseHttpLogging();

// Request delegate middleware
// app.UseMiddleware<RequestTimingMiddleware>();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler();
}

app.UseStatusCodePages();

// Terminal middleware -> will never invoke the next middleware in the pipeline
// app.Run();


await app.InitializeDbAsync();

app.Run();