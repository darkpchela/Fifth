using System.Collections.Generic;
using System.Linq;

namespace Fifth.Models
{
    public class Game
    {
        public GameInstance GameInstance { get; }

        public SessionData GameData { get; }

        public bool IsReadyToStart { get; private set; }

        private List<ConnectionUser> players = new List<ConnectionUser>();

        public Game(SessionData gameData)
        {
            this.GameData = gameData;
            this.GameInstance = new GameInstance(gameData.Id.ToString());
        }

        public Game(SessionData gameData, GameInstance gameInstance)
        {
            this.GameData = gameData;
            this.GameInstance = gameInstance;
        }

        public void Start()
        {
            GameData.Started = true;
            GameInstance.StartGame(players.Select(p => p.ConnectionId).ToList());
        }

        public bool IsAlive()
        {
            if (GameInstance is null || GameData is null)
                return false;
            return true;
        }

        public bool RegistPlayer(string connectionId, User user)
        {
            if (players.Count >= 2 || user is null || string.IsNullOrEmpty(connectionId))
                return false;
            var player = new ConnectionUser(user, connectionId);
            players.Add(player);
            if (players.Count == 2)
                IsReadyToStart = true;
            return true;
        }
    }
}