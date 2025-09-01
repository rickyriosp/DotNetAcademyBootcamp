using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Data;

public class GameDataLogger(GameStoreContext dbContext, ILogger<GameDataLogger> logger)
{
    // private readonly GameStoreData _data = data;
    // private readonly ILogger<GameDataLogger> _logger = logger;

    public async Task PrintGamesAsync()
    {
        foreach (var game in await dbContext.Games.ToListAsync())
        {
            logger.LogInformation("Game Id: {GameId} | Game Name: {GameName}", game.Id, game.Name);
        }
    }
}