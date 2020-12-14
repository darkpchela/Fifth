using System;
using System.Collections.Generic;

namespace Fifth.Models
{
    public class GameInstance
    {
        private int moveValue = -1;

        private int movesCount = 0;

        private int[] map = new int[9];

        private string PlayerX;

        private List<string> players = new List<string>();

        public string Id { get; }

        public string CurrentPlayer { get; private set; }

        public int PlayersCount
        {
            get
            {
                return players.Count;
            }
        }

        public GameInstance(string id)
        {
            this.Id = id;
        }

        public bool TryMakeMove(int postion, string player)
        {
            if (player != CurrentPlayer || postion < 0 || postion > 8 || map[postion] != 0 || movesCount >= 9)
                return false;
            map[postion] = moveValue;
            SwapMoveValue();
            SwapCurrentPlayer();
            return true;
        }

        public string CalcResult()
        {
            return "Not imnplemented!";
        }

        public void StartGame()
        {
            Random rnd = new Random();
            var index = rnd.Next(0, 1);
            CurrentPlayer = PlayerX = players[index];
        }

        public bool RegistPlayer(string connectionId)
        {
            if (players.Count >= 2)
                return false;
            players.Add(connectionId);
            return true;
        }

        public void KickPlayer(string connectionId)
        {
            players.Remove(connectionId);
        }

        private void SwapMoveValue()
        {
            if (moveValue == -1)
                moveValue = 1;
            else
                moveValue = 1;
            movesCount++;
        }

        private void SwapCurrentPlayer()
        {
            if (CurrentPlayer == players[0])
                CurrentPlayer = players[1];
            else
                CurrentPlayer = players[0];
        }

        private bool CheckLines(int[] array, int rowLength)
        {
            bool isMathed = false;
            for (int i = 0; i < array.Length / rowLength; i++)
            {
                for (int j = 1; j < rowLength; j++)
                {
                    if (array[i * rowLength + j] == array[i * rowLength + j - 1])
                        isMathed = true;
                    else
                    {
                        isMathed = false;
                        break;
                    }
                }
                if (isMathed)
                    break;
            }
            return isMathed;
        }

        private bool CheckColumns(int[] array, int rowLength)
        {
            bool isMathed = false;
            for (int i = 0; i < array.Length / rowLength; i++)
            {
                for (int j = 1; j < rowLength; j++)
                {
                    if (array[i + rowLength * j] == array[i + rowLength * (j - 1)])
                        isMathed = true;
                    else
                    {
                        isMathed = false;
                        break;
                    }
                }
                if (isMathed)
                    break;
            }
            return isMathed;
        }

        private bool CheckDiagonals(int[] array, int rank)
        {
            bool isMatched = false;
            for (int i = 0; i < 2; i++)
            {
                for (int j = 1; j < rank; j++)
                {
                    int k = (int)Math.Pow(-1, i);
                    if (array[rank * j + j * k + (rank - 1) * i] == array[rank * (j - 1) + (j - 1) * k + (rank - 1) * i])
                        isMatched = true;
                    else
                    {
                        isMatched = false;
                        break;
                    }
                }
                if (isMatched)
                    break;
            }
            return isMatched;
        }
    }
}