using System.ComponentModel.DataAnnotations;
using GameStore.Api.Models;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

const string GetGameEndpointName = "GetGame";

List<Genre> genres = new List<Genre>
{
    new Genre
    {
        Id = new Guid("66df989d-1d80-4081-90ea-c1b0c72dea5e"),
        Name = "Fighting"
    },
    new Genre
    {
        Id = new Guid("6d804700-a36e-40d0-9367-7d013809ed74"),
        Name = "Role-Playing"
    },
    new Genre
    {
        Id = new Guid("38f8633e-6fec-4635-b253-4cfc5e248d16"),
        Name = "Action-Adventure"
    },
    new Genre
    {
        Id = new Guid("254426b7-23a4-404f-9b01-f80f754109aa"),
        Name = "Sports"
    }
};

List<Game> games = new List<Game>
{
    new Game
    {
        Id = Guid.NewGuid(),
        Name = "Street Fighter II",
        Genre = genres[0],
        Price = 19.99m,
        ReleaseDate = new DateOnly(1992, 7, 15),
        Description = "A classic fighting game that revolutionized the genre."
    },
    new Game
    {
        Id = Guid.NewGuid(),
        Name = "Final Fantasy XIV",
        Genre = genres[1],
        Price = 59.99m,
        ReleaseDate = new DateOnly(2010, 9, 30),
        Description = "A massively multiplayer online role-playing game set in the world of Eorzea."
    },
    new Game
    {
        Id = Guid.NewGuid(),
        Name = "The Legend of Zelda: Breath of the Wild",
        Genre = genres[2],
        Price = 49.99m,
        ReleaseDate = new DateOnly(2017, 3, 3),
        Description = "An open-world action-adventure game set in the land of Hyrule."
    }
};

app.MapGet("/", () => "Hello World!");

// GET /games
app.MapGet("/games", () => games.Select(g => new GameSummaryDto(
    g.Id,
    g.Name,
    g.Genre.Name,
    g.Price,
    g.ReleaseDate)).ToList());

// GET /games/{id}
app.MapGet("/games/{id}", (Guid id) =>
{
    Game? game = games.Find(g => g.Id == id);

    return game is null ? Results.NotFound() : Results.Ok(new GameDetailsDto(
        game.Id,
        game.Name,
        game.Genre.Id,
        game.Price,
        game.ReleaseDate,
        game.Description));
})
.WithName(GetGameEndpointName);

// POST /games
app.MapPost("/games", (CreateGameDto gameDto) =>
{
    Genre? genre = genres.Find(g => g.Id == gameDto.GenreId);

    if (genre is null)
    {
        return Results.BadRequest($"Genre with ID {gameDto.GenreId} does not exist.");
    }

    Game game = new Game
    {
        Id = Guid.NewGuid(),
        Name = gameDto.Name,
        Genre = genre,
        Price = gameDto.Price,
        ReleaseDate = gameDto.ReleaseDate,
        Description = gameDto.Description
    };

    games.Add(game);

    return Results.CreatedAtRoute(
        GetGameEndpointName,
        new { id = game.Id },
        new GameDetailsDto(
            game.Id,
            game.Name,
            game.Genre.Id,
            game.Price,
            game.ReleaseDate,
            game.Description)
        );
})
.WithParameterValidation();

// PUT /games/{id}
app.MapPut("/games/{id}", (Guid id, UpdateGameDto updatedGameDto) =>
{
    Game? existingGame = games.Find(g => g.Id == id);

    if (existingGame is null)
    {
        return Results.NotFound();
    }

    Genre? genre = genres.Find(g => g.Id == updatedGameDto.GenreId);

    if (genre is null)
    {
        return Results.BadRequest($"Genre with ID {updatedGameDto.GenreId} does not exist.");
    }

    existingGame.Name = updatedGameDto.Name;
    existingGame.Genre = genre;
    existingGame.Price = updatedGameDto.Price;
    existingGame.ReleaseDate = updatedGameDto.ReleaseDate;
    existingGame.Description = updatedGameDto.Description;

    return Results.NoContent();
})
.WithParameterValidation();

// DELETE /games/{id}
app.MapDelete("/games/{id}", (Guid id) =>
{
    games.RemoveAll(g => g.Id == id);

    return Results.NoContent();
});

// GET /genres
app.MapGet("/genres", () => genres.Select(g => new GenreDto(g.Id, g.Name)).ToList());

app.Run();

public record GameDetailsDto(
    Guid Id,
    string Name,
    Guid GenreId,
    decimal Price,
    DateOnly ReleaseDate,
    string Description);

public record GameSummaryDto(
    Guid Id,
    string Name,
    string Genre,
    decimal Price,
    DateOnly ReleaseDate);

public record CreateGameDto(
    [Required]
    [StringLength(50)]
    string Name,
    Guid GenreId,
    [Range(1, 100)]
    decimal Price,
    DateOnly ReleaseDate,
    [Required]
    [StringLength(500)]
    string Description);

public record UpdateGameDto(
    [Required]
    [StringLength(50)]
    string Name,
    Guid GenreId,
    [Range(1, 100)]
    decimal Price,
    DateOnly ReleaseDate,
    [Required]
    [StringLength(500)]
    string Description);

public record GenreDto(
    Guid Id,
    string Name);