using GameStore.Api.Models;

using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Data;

public static class DataExtensions
{
    public static async Task InitializeDbAsync(this WebApplication app)
    {
        await app.MigrateDbAsync();
        await app.SeedDbAsync();
        app.Logger.LogTrace(18, "Database initialized!");
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
                new Genre { Name = "Sports" },
                new Genre { Name = "Racing" }
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
            },
            new Game
            {
                Name = "FIFA 25",
                // Genre = genres.Find(g => g.Name == "Sports"),
                GenreId = genres.Find(g => g.Name == "Sports")!.Id,
                Price = 69.99m,
                ReleaseDate = new DateOnly(2024, 9, 27),
                Description = "The latest installment in the popular football simulation series."
            },
            new Game
            {
                Name = "Forza Horizon 5",
                // Genre = genres.Find(g => g.Name == "Racing"),
                GenreId = genres.Find(g => g.Name == "Racing")!.Id,
                Price = 59.99m,
                ReleaseDate = new DateOnly(2021, 11, 9),
                Description = "An open-world racing game set in a fictionalized version of Mexico."
            },
            new Game
            {
                Name = "Mario Kart 8 Deluxe",
                // Genre = genres.Find(g => g.Name == "Racing"),
                GenreId = genres.Find(g => g.Name == "Racing")!.Id,
                Price = 59.99m,
                ReleaseDate = new DateOnly(2017, 4, 28),
                Description = "A fun and exciting kart racing game featuring popular Nintendo characters."
            }
        );

        await dbContext.SaveChangesAsync();
    }
}