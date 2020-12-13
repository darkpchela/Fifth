using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fifth.Models
{
    public class Game
    {
        private int moveValue = -1;

        private int movesCount = 0;

        private int[] map = new int[9];

        private List<string> players = new List<string>();

        public string Id { get; }

        public int PlayersCount { get; } = 0;

        public string CurrentPlayer { get; private set; }

        public Game(string id)
        {
            this.Id = id;
        }

        public bool TryMakeMove(int postion, string player)
        {
            if (player != CurrentPlayer || postion < 0 || postion > 8 || map[postion]!=0 || movesCount >= 9)
                return false;
            map[postion] = moveValue;
            SwapMoveValue();
            return true;
        }

        public string CalcResult()
        {
            return "Not implemted";
        }

        public void RegistPlayer(string connectionId)
        {
            if(PlayersCount < 2)
            players.Add(connectionId);
        }

        private void SwapMoveValue()
        {
            if (moveValue == -1)
                moveValue = 1;
            else
                moveValue = 1;

            movesCount++;
        }
    }
}
