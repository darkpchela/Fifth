namespace Fifth.Models
{
    public class GameSession
    {
        public GameProcess Instance { get; }

        public SessionData Data { get; }

        public GameSession(SessionData gameData)
        {
            this.Data = gameData;
            this.Instance = new GameProcess(gameData.Id.ToString());
        }

        public GameSession(SessionData gameData, GameProcess gameInstance)
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