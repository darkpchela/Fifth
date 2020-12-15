namespace Fifth.Models
{
    public class GameSession
    {
        public GameInstance GameInstance { get; }

        public SessionData GameData { get; }

        public GameSession(SessionData gameData)
        {
            this.GameData = gameData;
            this.GameInstance = new GameInstance(gameData.Id.ToString());
        }

        public GameSession(SessionData gameData, GameInstance gameInstance)
        {
            this.GameData = gameData;
            this.GameInstance = gameInstance;
        }

        public void Start()
        {
            GameData.Started = true;
            GameInstance.StartGame();
        }

        public bool IsAlive()
        {
            if (GameInstance is null || GameData is null)
                return false;
            return true;
        }
    }
}