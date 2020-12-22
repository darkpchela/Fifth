using System.Collections.Generic;

namespace Fifth.Models
{
    public class GameState
    {
        public char MoveValue { get; set; }

        public int MovesCount { get; set; }

        public char?[] Map { get; set; }

        public Dictionary<char, UserConnection> CharToPlayer { get; set; }

        public List<UserConnection> Players { get; set; }

        public UserConnection CurrentPlayer { get; set; }
    }
}