using GameStore.Api.Data;

using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Features.Games.GetGames;

public static class GetGamesEndpoint
{
    public static void MapGetGames(this IEndpointRouteBuilder app)
    {
        // GET /games
        app.MapGet("/", async (GameStoreContext dbContext, [AsParameters] GetGamesDto request) =>
            {
                var skipCount = (request.PageNumber - 1) * request.PageSize;
                // skipCount = (1 - 1) * 5 = 0 -> (2 - 1) * 5 = 5 -> (3 - 1) * 5 = 10

                var filteredGames = dbContext.Games
                    .Where(game => string.IsNullOrWhiteSpace(request.Name) ||
                                   // game.Name.Contains(request.Name, StringComparison.OrdinalIgnoreCase)  // -> not translatable to SQL
                                   EF.Functions.Like(game.Name, $"%{request.Name}%") // -> translatable to SQL
                    );

                var gamesOnPage = await filteredGames
                    .OrderBy(game => game.Name)
                    .Skip(skipCount)
                    .Take(request.PageSize)
                    .Include(game => game.Genre)
                    .Select(game => new GameSummaryDto(
                        game.Id,
                        game.Name,
                        game.Genre!.Name, // ! -> null forgiveness operator
                        game.Price,
                        game.ReleaseDate,
                        game.ImageUri
                    ))
                    .AsNoTracking()
                    .ToListAsync();

                var totalGames = await filteredGames.CountAsync();
                var totalPages = (int)Math.Ceiling(totalGames / (double)request.PageSize);

                return new GamesPageDto(totalPages, gamesOnPage);
            }
        );
    }
}