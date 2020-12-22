using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fifth.Models
{
    public class GameMaster : IDisposable
    {
        private readonly GameProcess gameProcess;

        private readonly GameState before;

        private readonly GameState after;

        public GameMaster(GameProcess gameProcess)
        {
            this.gameProcess = gameProcess;
            this.before = gameProcess.GetState();
            this.after = gameProcess.GetState();
        }

        public MoveResult MakeMove(int postion, string connectionId)
        {
            if (connectionId != before.CurrentPlayer.ConnectionId || postion < 0 || postion > 8 || before.Map[postion] != null || before.MovesCount >= 9)
                return new MoveResult(false);
            after.Map[postion] = before.MoveValue;
            SwapMoveValue();
            SwapCurrentPlayer();
            MoveResult res = CalcResult();
            gameProcess.SetState(after);
            return res;
        }

        public bool PrepareToStart()
        {
            if (before.Players.Count != 2)
                return false;
            Random rnd = new Random();
            var index = rnd.Next(0, 2);
            after.CurrentPlayer = before.Players[index];
            after.CharToPlayer.Add('x', after.CurrentPlayer);
            after.CharToPlayer.Add('o', after.Players.FirstOrDefault(u => u != after.CurrentPlayer));
            gameProcess.SetState(after);
            return true;
        }

        public bool RegistPlayer(string connectionId, string userName)
        {
            if (before.Players.Count >= 2)
                return false;
            var connection = new UserConnection(userName, connectionId);
            after.Players.Add(connection);
            gameProcess.SetState(after);
            return true;
        }

        private void SwapMoveValue()
        {
            if (before.MoveValue == 'x')
                after.MoveValue = 'o';
            else
                after.MoveValue = 'x';
            after.MovesCount++;
        }

        private void SwapCurrentPlayer()
        {
            if (before.CurrentPlayer == before.Players[0])
                after.CurrentPlayer = before.Players[1];
            else
                after.CurrentPlayer = before.Players[0];
        }

        private MoveResult CalcResult()
        {
            if (!CheckLines(before.Map, 3, out char? value))
                if (!CheckColumns(before.Map, 3, out value))
                    if (!CheckDiagonals(before.Map, 3, out value))
                        if (before.MovesCount < 9)
                            return new MoveResult(true);
                        else
                            return new MoveResult();
            return new MoveResult($"{before.CharToPlayer[value.Value].UserName}");
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

        #region Disposable

        private bool disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                disposed = true;
            }
        }

        ~GameMaster()
        {
            Dispose(false);
        }

        #endregion
    }
}
