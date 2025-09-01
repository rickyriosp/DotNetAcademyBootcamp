using GameStore.Api.Models;

using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Data;

public static class DataExtensions
{
    public static async Task InitializeDbAsync(this WebApplication app)
    {
        await app.MigrateDbAsync();
        await app.SeedDbAsync();
    }

    private static async Task MigrateDbAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        GameStoreContext dbContext = scope.ServiceProvider.GetRequiredService<GameStoreContext>();

        await dbContext.Database.MigrateAsync();
    }

    private static async Task SeedDbAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        GameStoreContext dbContext = scope.ServiceProvider.GetRequiredService<GameStoreContext>();

        if (!dbContext.Genres.Any())
        {
            dbContext.Genres.AddRange(
                new Genre { Name = "Fighting" },
                new Genre { Name = "Role-Playing" },
                new Genre { Name = "Action-Adventure" },
                new Genre { Name = "Sports" }
            );

            await dbContext.SaveChangesAsync();
        }

        if (dbContext.Games.Any())
        {
            return;
        }

        var genres = dbContext.Genres.ToList();

        dbContext.Games.AddRange(
            new Game
            {
                Name = "Street Fighter II",
                // Genre = genres.Find(g => g.Name == "Fighting"),
                GenreId = genres.Find(g => g.Name == "Fighting")!.Id,
                Price = 19.99m,
                ReleaseDate = new DateOnly(1992, 7, 15),
                Description = "A classic fighting game that revolutionized the genre."
            },
            new Game
            {
                Name = "Final Fantasy XIV",
                // Genre = genres.Find(g => g.Name == "Role-Playing"),
                GenreId = genres.Find(g => g.Name == "Role-Playing")!.Id,
                Price = 59.99m,
                ReleaseDate = new DateOnly(2010, 9, 30),
                Description =
                    "A massively multiplayer online role-playing game set in the world of Eorzea."
            },
            new Game
            {
                Name = "The Legend of Zelda: Breath of the Wild",
                // Genre = genres.Find(g => g.Name == "Action-Adventure"),
                GenreId = genres.Find(g => g.Name == "Action-Adventure")!.Id,
                Price = 49.99m,
                ReleaseDate = new DateOnly(2017, 3, 3),
                Description = "An open-world action-adventure game set in the land of Hyrule."
            }
        );

        await dbContext.SaveChangesAsync();
    }
}