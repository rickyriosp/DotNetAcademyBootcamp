using GameStore.Api.Data;
using GameStore.Api.Models;

namespace GameStore.Api.Features.Games.UpdateGame;

public static class UpdateGameEndpoint
{
    public static void MapUpdateGame(this IEndpointRouteBuilder app)
    {
        // PUT /games/{id}
        app.MapPut("/{id}", (Guid id, UpdateGameDto updatedGameDto, GameStoreData data) =>
            {
                Game? existingGame = data.GetGame(id);

                if (existingGame is null)
                {
                    return Results.NotFound();
                }

                Genre? genre = data.GetGenre(updatedGameDto.GenreId);

                if (genre is null)
                {
                    return Results.BadRequest(
                        $"Genre with ID {updatedGameDto.GenreId} does not exist."
                    );
                }

                existingGame.Name = updatedGameDto.Name;
                existingGame.Genre = genre;
                existingGame.Price = updatedGameDto.Price;
                existingGame.ReleaseDate = updatedGameDto.ReleaseDate;
                existingGame.Description = updatedGameDto.Description;

                return Results.NoContent();
            })
            .WithParameterValidation();
    }
}