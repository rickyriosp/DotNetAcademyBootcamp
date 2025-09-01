using GameStore.Api.Data;
using GameStore.Api.Models;

namespace GameStore.Api.Features.Games.UpdateGame;

public static class UpdateGameEndpoint
{
    public static void MapUpdateGame(this IEndpointRouteBuilder app)
    {
        // PUT /games/{id}
        app.MapPut("/{id}", (Guid id, UpdateGameDto updatedGameDto, GameStoreContext dbContext) =>
            {
                Game? existingGame = dbContext.Games.Find(id);

                if (existingGame is null)
                {
                    return Results.NotFound();
                }

                existingGame.Name = updatedGameDto.Name;
                existingGame.GenreId = updatedGameDto.GenreId;
                existingGame.Price = updatedGameDto.Price;
                existingGame.ReleaseDate = updatedGameDto.ReleaseDate;
                existingGame.Description = updatedGameDto.Description;

                dbContext.SaveChanges();

                return Results.NoContent();
            })
            .WithParameterValidation();
    }
}