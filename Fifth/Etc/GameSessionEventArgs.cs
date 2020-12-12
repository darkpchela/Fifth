using Fifth.Models;
using System;

namespace Fifth.Etc
{
    public delegate void GameSessionEventHandler(object sender, GameSessionEventArgs gameSessionEventArgs);

    public class GameSessionEventArgs : EventArgs
    {
        public GameSession GameSession { get; }

        public GameSessionEventArgs(GameSession gameSession)
        {
            GameSession = gameSession;
        }
    }
}