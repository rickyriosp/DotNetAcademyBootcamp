namespace GameStore.Api.Data;

public class GameDataLogger(GameStoreData data, ILogger<GameDataLogger> logger)
{
    // private readonly GameStoreData _data = data;
    // private readonly ILogger<GameDataLogger> _logger = logger;

    public void PrintGames()
    {
        foreach (var game in data.GetGames())
        {
            logger.LogInformation("Game Id: {GameId} | Game Name: {GameName}", game.Id, game.Name);
        }
    }
}