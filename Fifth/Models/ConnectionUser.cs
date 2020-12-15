namespace Fifth.Models
{
    public class ConnectionUser
    {
        public string ConnectionId { get; }

        public User User { get; }

        public ConnectionUser(User user, string connectionId)
        {
            this.ConnectionId = connectionId;
            this.User = user;
        }
    }
}