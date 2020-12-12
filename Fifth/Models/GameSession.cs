using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fifth.Models
{
    public class GameSession
    {
        public string Owner { get; set; }

        public string Opponent { get; set; }

        public string SessionName { get; set; }

        public string SessionId { get; set; }

        public IList<string> Tags { get; set; }
    }
}
