using Fifth.Models;
using System;

namespace Fifth.Etc
{
    public delegate void GameSessionEventHandler(object sender, GameSessionEventArgs gameSessionEventArgs);

    public class GameSessionEventArgs : EventArgs
    {
        public GameData GameSession { get; }

        public GameSessionEventArgs(GameData gameSession)
        {
            GameSession = gameSession;
        }
    }
}