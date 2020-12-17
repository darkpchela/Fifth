namespace Fifth.Models
{
    public class MoveResult
    {
        public bool MoveMaid { get; }

        public bool GameFinished { get; }

        public bool IsDraw { get; }

        public string Winner { get; }

        public MoveResult(bool moveMaid)
        {
            this.MoveMaid = moveMaid;
            GameFinished = false;
        }

        public MoveResult(string winner = null)
        {
            MoveMaid = true;
            GameFinished = true;
            Winner = winner;
            if (winner is null)
                IsDraw = true;
        }
    }
}