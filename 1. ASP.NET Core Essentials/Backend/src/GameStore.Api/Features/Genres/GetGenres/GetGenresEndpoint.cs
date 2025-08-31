using GameStore.Api.Data;
using GameStore.Api.Features.Games.GetGenres;

namespace GameStore.Api.Features.Genres.GetGenres;

public static class GetGenresEndpoint
{
    public static void MapGetGenres(this IEndpointRouteBuilder app, GameStoreData data)
    {
        // GET /genres
        app.MapGet(
            "/",
            () => data.GetGenres().Select(g => new GenreDto(g.Id, g.Name)).ToList()
        );
    }
}
