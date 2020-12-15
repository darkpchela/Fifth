namespace Fifth.Models
{
    public class MoveResult
    {
        public bool MoveMaid { get; }

        public bool GameFinished { get; }

        public string Result { get; }

        public MoveResult(bool moveMaid)
        {
            this.MoveMaid = moveMaid;
            GameFinished = false;
        }

        public MoveResult(string result)
        {
            MoveMaid = true;
            GameFinished = true;
            Result = result;
        }
    }
}