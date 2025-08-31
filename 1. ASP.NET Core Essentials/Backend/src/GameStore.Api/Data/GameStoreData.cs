using GameStore.Api.Models;

namespace GameStore.Api.Data;

public class GameStoreData
{
    private readonly List<Genre> genres = new List<Genre>
    {
        new Genre { Id = new Guid("66df989d-1d80-4081-90ea-c1b0c72dea5e"), Name = "Fighting" },
        new Genre { Id = new Guid("6d804700-a36e-40d0-9367-7d013809ed74"), Name = "Role-Playing" },
        new Genre
        {
            Id = new Guid("38f8633e-6fec-4635-b253-4cfc5e248d16"),
            Name = "Action-Adventure",
        },
        new Genre { Id = new Guid("254426b7-23a4-404f-9b01-f80f754109aa"), Name = "Sports" },
    };

    private readonly List<Game> games;

    public GameStoreData()
    {
        games = new List<Game>
        {
            new Game
            {
                Id = Guid.NewGuid(),
                Name = "Street Fighter II",
                Genre = genres[0],
                Price = 19.99m,
                ReleaseDate = new DateOnly(1992, 7, 15),
                Description = "A classic fighting game that revolutionized the genre.",
            },
            new Game
            {
                Id = Guid.NewGuid(),
                Name = "Final Fantasy XIV",
                Genre = genres[1],
                Price = 59.99m,
                ReleaseDate = new DateOnly(2010, 9, 30),
                Description =
                    "A massively multiplayer online role-playing game set in the world of Eorzea.",
            },
            new Game
            {
                Id = Guid.NewGuid(),
                Name = "The Legend of Zelda: Breath of the Wild",
                Genre = genres[2],
                Price = 49.99m,
                ReleaseDate = new DateOnly(2017, 3, 3),
                Description = "An open-world action-adventure game set in the land of Hyrule.",
            },
        };
    }

    public IEnumerable<Game> GetGames() => games; // => games : known as expression body member

    public Game? GetGame(Guid id) => games.Find(g => g.Id == id);

    public void AddGame(Game game)
    {
        game.Id = Guid.NewGuid();
        games.Add(game);
    }

    public void RemoveGame(Guid id) => games.RemoveAll(g => g.Id == id);

    public IEnumerable<Genre> GetGenres() => genres;

    public Genre? GetGenre(Guid id) => genres.Find(g => g.Id == id);
}
