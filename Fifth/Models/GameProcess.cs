using System;
using System.Collections.Generic;
using System.Linq;

namespace Fifth.Models
{
    public class GameProcess
    {
        private char moveValue = 'x';

        private int movesCount = 0;

        private char?[] map = new char?[9];

        private List<UserConnection> players = new List<UserConnection>();

        private Dictionary<char, UserConnection> charToPlayer = new Dictionary<char, UserConnection>();

        private UserConnection currentPlayer;

        public string Id { get; }

        public GameProcess(string id)
        {
            this.Id = id;
        }

        public GameState GetState()
        {
            return new GameState
            {
                MoveValue = moveValue,
                MovesCount = movesCount,
                Map = map,
                Players = players,
                CharToPlayer = charToPlayer,
                CurrentPlayer = currentPlayer
            };
        }

        public void SetState(GameState state)
        {
            moveValue = state.MoveValue;
            movesCount = state.MovesCount;
            map = state.Map;
            players = state.Players;
            charToPlayer = state.CharToPlayer;
            currentPlayer = state.CurrentPlayer;
        }
    }
}