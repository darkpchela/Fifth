using System.Collections.Generic;

namespace Fifth.ViewModels
{
    public class GameSessionVM
    {
        public int Id { get; set; }
        public string GameName { get; set; }

        public string UserName { get; set; }

        public IList<string> Tags { get; set; }
    }
}