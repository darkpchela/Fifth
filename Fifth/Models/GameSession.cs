namespace Fifth.Models
{
    public class GameSession
    {
        public GameInstance Instance { get; }

        public SessionData Data { get; }

        public GameSession(SessionData gameData)
        {
            this.Data = gameData;
            this.Instance = new GameInstance(gameData.Id.ToString());
        }

        public GameSession(SessionData gameData, GameInstance gameInstance)
        {
            this.Data = gameData;
            this.Instance = gameInstance;
        }

        public bool IsAlive()
        {
            if (Instance is null || Data is null)
                return false;
            return true;
        }
    }
}