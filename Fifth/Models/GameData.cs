#nullable disable

namespace Fifth.Models
{
    public partial class GameData
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int CreatorId { get; set; }

        public bool Started { get; set; }

        public virtual User Creator { get; set; }

        public GameData()
        {
        }

        public GameData(string gameName, User userCreator)
        {
            Creator = userCreator;
            Name = gameName;
        }
    }
}