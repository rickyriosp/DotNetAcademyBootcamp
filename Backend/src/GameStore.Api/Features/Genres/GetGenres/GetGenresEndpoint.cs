using GameStore.Api.Data;

using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Features.Genres.GetGenres;

public static class GetGenresEndpoint
{
    public static void MapGetGenres(this IEndpointRouteBuilder app)
    {
        // GET /genres
        app.MapGet("/", async (GameStoreContext dbContext) => await dbContext.Genres
            .Select(g => new GenreDto(g.Id, g.Name))
            .AsNoTracking()
            .ToListAsync()
        );
    }
}