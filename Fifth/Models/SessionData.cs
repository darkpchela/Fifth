using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Fifth.Models
{
    [Table("Sessions")]
    public partial class SessionData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CreatorId { get; set; }
        public bool Started { get; set; }

        public virtual User Creator { get; set; }

        public SessionData()
        {
        }

        public SessionData(string name, User userCreator)
        {
            this.Name = name;
            this.Creator = userCreator;
        }
    }
}