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

        private List<UserConnection> players;

        private Dictionary<char, UserConnection> charUser = new Dictionary<char, UserConnection>();

        public string Id { get; }

        public UserConnection CurrentPlayer { get; private set; }

        public List<UserConnection> Players
        {
            get
            {
                return players ?? (players = new List<UserConnection>());
            }
        }


        public GameProcess(string id)
        {
            this.Id = id;
        }

        public MoveResult MakeMove(int postion, string connectionId)
        {
            if (connectionId != CurrentPlayer.ConnectionId || postion < 0 || postion > 8 || map[postion] != null || movesCount >= 9)
                return new MoveResult(false);
            map[postion] = moveValue;
            SwapMoveValue();
            SwapCurrentPlayer();
            MoveResult res = CalcResult();
            return res;
        }

        public void PrepareToStart()
        {
            Random rnd = new Random();
            var index = rnd.Next(0, 2);
            CurrentPlayer = players[index];
            charUser.Add('x', CurrentPlayer);
            charUser.Add('o', players.FirstOrDefault(u => u != CurrentPlayer));
        }

        private void SwapMoveValue()
        {
            if (moveValue == 'x')
                moveValue = 'o';
            else
                moveValue = 'x';
            movesCount++;
        }

        private void SwapCurrentPlayer()
        {
            if (CurrentPlayer == players[0])
                CurrentPlayer = players[1];
            else
                CurrentPlayer = players[0];
        }

        private MoveResult CalcResult()
        {
            if (!CheckLines(map, 3, out char? value))
                if (!CheckColumns(map, 3, out value))
                    if (!CheckDiagonals(map, 3, out value))
                        if (movesCount < 9)
                            return new MoveResult(true);
                        else
                            return new MoveResult();
            return new MoveResult($"{charUser[value.Value].UserName}");
        }

        private bool CheckLines(char?[] array, int rowLength, out char? value)
        {
            bool isMathed = false;
            value = null;
            for (int i = 0; i < array.Length / rowLength; i++)
            {
                for (int j = 1; j < rowLength; j++)
                {
                    if (array[i * rowLength + j] != null && array[i * rowLength + j] == array[i * rowLength + j - 1])
                    {
                        isMathed = true;
                        value = array[i * rowLength + j];
                    }
                    else
                    {
                        value = null;
                        isMathed = false;
                        break;
                    }
                }
                if (isMathed)
                    break;
            }
            return isMathed;
        }

        private bool CheckColumns(char?[] array, int rowLength, out char? value)
        {
            bool isMathed = false;
            value = null;
            for (int i = 0; i < array.Length / rowLength; i++)
            {
                for (int j = 1; j < rowLength; j++)
                {
                    if (array[i + rowLength * j] != null && array[i + rowLength * j] == array[i + rowLength * (j - 1)])
                    {
                        value = array[i + rowLength * j];
                        isMathed = true;
                    }
                    else
                    {
                        value = null;
                        isMathed = false;
                        break;
                    }
                }
                if (isMathed)
                    break;
            }
            return isMathed;
        }

        private bool CheckDiagonals(char?[] array, int rank, out char? value)
        {
            bool isMatched = false;
            value = null;
            for (int i = 0; i < 2; i++)
            {
                for (int j = 1; j < rank; j++)
                {
                    int k = (int)Math.Pow(-1, i);
                    if (array[rank * j + j * k + (rank - 1) * i] != null && array[rank * j + j * k + (rank - 1) * i] == array[rank * (j - 1) + (j - 1) * k + (rank - 1) * i])
                    {
                        value = array[rank * j + j * k + (rank - 1) * i];
                        isMatched = true;
                    }
                    else
                    {
                        value = null;
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