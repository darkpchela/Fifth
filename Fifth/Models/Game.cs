namespace Fifth.Models
{
    public class Game
    {
        public GameInstance GameInstance { get; }

        public GameData GameData { get; }

        public Game(GameData gameData)
        {
            this.GameData = gameData;
            this.GameInstance = new GameInstance(gameData.Id.ToString());
        }

        public Game(GameData gameData, GameInstance gameInstance)
        {
            this.GameData = gameData;
            this.GameInstance = gameInstance;
        }
    }
}