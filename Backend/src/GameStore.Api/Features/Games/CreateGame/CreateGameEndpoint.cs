using GameStore.Api.Data;
using GameStore.Api.Features.Games.Constants;
using GameStore.Api.Models;
using GameStore.Api.Shared.FileUpload;

using Microsoft.AspNetCore.Mvc;

namespace GameStore.Api.Features.Games.CreateGame;

public static class CreateGameEndpoint
{
    private const string DefaultImageUri = "https://placehold.co/100";

    public static void MapCreateGame(this IEndpointRouteBuilder app)
    {
        // POST /games
        app.MapPost("/", async (
                [FromForm] CreateGameDto gameDto,
                GameStoreContext dbContext,
                ILoggerFactory loggerFactory,
                FileUploader fileUploader) =>
            {
                var imageUri = DefaultImageUri;

                if (gameDto.ImageFile is not null)
                {
                    var fileUploadResult =
                        await fileUploader.UploadFileAsync(gameDto.ImageFile, StorageNames.GamesImagesFolder);

                    if (!fileUploadResult.IsSuccess)
                    {
                        return Results.BadRequest(new { message = fileUploadResult.ErrorMessage });
                    }

                    imageUri = fileUploadResult.FileUrl;
                }

                Game game = new()
                {
                    Name = gameDto.Name,
                    GenreId = gameDto.GenreId,
                    Price = gameDto.Price,
                    ReleaseDate = gameDto.ReleaseDate,
                    Description = gameDto.Description,
                    ImageUri = imageUri!
                };

                dbContext.Games.Add(game);
                await dbContext.SaveChangesAsync();

                var logger = loggerFactory.CreateLogger("Games");
                logger.LogInformation("Created game {gameName} with price {gamePrice}", game.Name, game.Price);

                return Results.CreatedAtRoute(
                    EndpointNames.GetGame,
                    new { id = game.Id },
                    new GameDetailsDto(
                        game.Id,
                        game.Name,
                        game.GenreId,
                        game.Price,
                        game.ReleaseDate,
                        game.Description,
                        game.ImageUri
                    )
                );
            })
            .WithParameterValidation()
            .DisableAntiforgery();

        // Antiforgery tries to prevent CSRF attacks by ensuring that a request made on behalf of an authenticated user is intentional.
        // It does this by requiring a special token to be included in requests that modify data (like POST, PUT, DELETE).
        // In APIs, especially those consumed by third-party clients or mobile apps, CSRF is less of a concern because these clients are not typically authenticated users in the same way a web browser is.
        // Therefore, it's common to disable antiforgery validation for API endpoints to avoid unnecessary complexity and potential issues with legitimate requests being blocked.
        // .RequireAntiForgery();

        // CSRF stands for Cross-Site Request Forgery. It's a type of attack where a malicious website tricks a user's browser into making an unwanted request to a different site where the user is authenticated.
        // In the context of APIs, especially RESTful APIs, CSRF protection is often not necessary because:
        // 1. APIs are typically stateless and do not rely on cookies for authentication.
        // 2. APIs are often consumed by third-party clients or mobile apps, which are not susceptible to CSRF attacks in the same way web browsers are.
        // 3. APIs usually use tokens (like JWT) for authentication, which are not automatically sent by browsers with each request.
        // Therefore, disabling CSRF protection for API endpoints can simplify the implementation and avoid potential issues with legitimate requests being blocked.
    }
}