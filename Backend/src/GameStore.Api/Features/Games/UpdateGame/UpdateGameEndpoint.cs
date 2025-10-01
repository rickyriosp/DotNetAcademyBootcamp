using GameStore.Api.Data;
using GameStore.Api.Features.Games.Constants;
using GameStore.Api.Models;
using GameStore.Api.Shared.FileUpload;

using Microsoft.AspNetCore.Mvc;

namespace GameStore.Api.Features.Games.UpdateGame;

public static class UpdateGameEndpoint
{
    public static void MapUpdateGame(this IEndpointRouteBuilder app)
    {
        // PUT /games/{id}
        app.MapPut("/{id}", async (
                Guid id,
                [FromForm] UpdateGameDto updatedGameDto,
                GameStoreContext dbContext,
                FileUploader fileUploader,
                GameDataLogger logger) =>
            {
                Game? existingGame = await dbContext.Games.FindAsync(id);

                if (existingGame is null)
                {
                    return Results.NotFound();
                }

                if (updatedGameDto.ImageFile is not null)
                {
                    var fileUploadResult =
                        await fileUploader.UploadFileAsync(updatedGameDto.ImageFile, StorageNames.GamesImagesFolder);

                    if (!fileUploadResult.IsSuccess)
                    {
                        return Results.BadRequest(new { message = fileUploadResult.ErrorMessage });
                    }

                    existingGame.ImageUri = fileUploadResult.FileUrl!;
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
            .WithParameterValidation()
            .DisableAntiforgery();
    }
}