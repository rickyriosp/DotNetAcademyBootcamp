using GameStore.Api.Data;
using GameStore.Api.Models;

namespace GameStore.Api.Features.Games.UpdateGame;

public static class UpdateGameEndpoint
{
    public static void MapUpdateGame(this IEndpointRouteBuilder app)
    {
        // PUT /games/{id}
        app.MapPut("/{id}",
                async (Guid id, UpdateGameDto updatedGameDto, GameStoreContext dbContext, GameDataLogger logger) =>
                {
                    Game? existingGame = await dbContext.Games.FindAsync(id);

                    if (existingGame is null)
                    {
                        return Results.NotFound();
                    }

                    existingGame.Name = updatedGameDto.Name;
                    existingGame.GenreId = updatedGameDto.GenreId;
                    existingGame.Price = updatedGameDto.Price;
                    existingGame.ReleaseDate = updatedGameDto.ReleaseDate;
                    existingGame.Description = updatedGameDto.Description;

                    await dbContext.SaveChangesAsync();

                    await logger.PrintGamesAsync();

                    return Results.NoContent();
                })
            .WithParameterValidation();
    }
}