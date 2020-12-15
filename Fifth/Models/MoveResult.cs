namespace Fifth.Models
{
    public class MoveResult
    {
        public bool MoveMaid { get;  }

        public bool GameFinished { get;  }

        public GameResult GameResult { get;  }

        public MoveResult(bool moveMaid)
        {
            this.MoveMaid = moveMaid;
            GameFinished = false;
        }

        public MoveResult(int result)
        {
            MoveMaid = true;
            GameFinished = true;
            GameResult = (GameResult)result;
        }
    }

    public enum GameResult
    {
        Draw,
        WinnerX,
        WinnerO
    }
}