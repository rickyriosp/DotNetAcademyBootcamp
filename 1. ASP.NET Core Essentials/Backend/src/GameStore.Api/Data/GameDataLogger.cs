namespace GameStore.Api.Data;

public class GameDataLogger(GameStoreContext dbContext, ILogger<GameDataLogger> logger)
{
    // private readonly GameStoreData _data = data;
    // private readonly ILogger<GameDataLogger> _logger = logger;

    public void PrintGames()
    {
        foreach (var game in dbContext.Games.ToList())
        {
            logger.LogInformation("Game Id: {GameId} | Game Name: {GameName}", game.Id, game.Name);
        }
    }
}