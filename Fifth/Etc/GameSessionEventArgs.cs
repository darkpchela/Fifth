using Fifth.Models;
using System;

namespace Fifth.Etc
{
    public delegate void GameSessionEventHandler(object sender, GameSessionEventArgs gameSessionEventArgs);

    public class GameSessionEventArgs : EventArgs
    {
        public GameInfoData GameSession { get; }

        public GameSessionEventArgs(GameInfoData gameSession)
        {
            GameSession = gameSession;
        }
    }
}