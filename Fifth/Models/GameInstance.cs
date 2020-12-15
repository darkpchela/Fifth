using System;
using System.Collections.Generic;
using System.Linq;

namespace Fifth.Models
{
    public class GameInstance
    {
        private int moveValue = 1;

        private int movesCount = 0;

        private int[] map = new int[9];

        private IList<string> players;

        public string Id { get; }

        public string CurrentPlayer { get; private set; }

        public GameInstance(string id)
        {
            this.Id = id;
        }

        public MoveResult MakeMove(int postion, string connectionId)
        {
            if (connectionId != CurrentPlayer || postion < 0 || postion > 8 || map[postion] != 0 || movesCount >= 9)
                return new MoveResult(false);
            map[postion] = moveValue;
            SwapMoveValue();
            SwapCurrentPlayer();
            int res = CalcResult();
            if (res == 0 && movesCount < 9)
                return new MoveResult(true);
            return new MoveResult(res);
        }


        public void StartGame(IList<string> connections)
        {
            players = connections;
            Random rnd = new Random();
            var index = rnd.Next(0, 2);
            CurrentPlayer = players[index];
        }

        private void SwapMoveValue()
        {
            if (moveValue == 1)
                moveValue = 2;
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
        private int CalcResult()
        {
            if (!CheckLines(map, 3, out int value))
                if (!CheckColumns(map, 3, out value))
                    CheckDiagonals(map, 3, out value);
            return value;
        }

        private bool CheckLines(int[] array, int rowLength, out int value)
        {
            bool isMathed = false;
            value = 0;
            for (int i = 0; i < array.Length / rowLength; i++)
            {
                for (int j = 1; j < rowLength; j++)
                {
                    if (array[i * rowLength + j] == array[i * rowLength + j - 1])
                    {
                        isMathed = true;
                        value = array[i * rowLength + j];
                    }
                    else
                    {
                        value = 0;
                        isMathed = false;
                        break;
                    }
                }
                if (isMathed)
                    break;
            }
            return isMathed;
        }

        private bool CheckColumns(int[] array, int rowLength, out int value)
        {
            bool isMathed = false;
            value = 0;
            for (int i = 0; i < array.Length / rowLength; i++)
            {
                for (int j = 1; j < rowLength; j++)
                {
                    if (array[i + rowLength * j] == array[i + rowLength * (j - 1)])
                    {
                        value = array[i + rowLength * j];
                        isMathed = true;
                    }
                    else
                    {
                        value = 0;
                        isMathed = false;
                        break;
                    }
                }
                if (isMathed)
                    break;
            }
            return isMathed;
        }

        private bool CheckDiagonals(int[] array, int rank, out int value)
        {
            bool isMatched = false;
            value = 0;
            for (int i = 0; i < 2; i++)
            {
                for (int j = 1; j < rank; j++)
                {
                    int k = (int)Math.Pow(-1, i);
                    if (array[rank * j + j * k + (rank - 1) * i] == array[rank * (j - 1) + (j - 1) * k + (rank - 1) * i])
                    {
                        value = array[rank * j + j * k + (rank - 1) * i];
                        isMatched = true;
                    }
                    else
                    {
                        value = 0;
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