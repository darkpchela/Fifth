namespace Fifth.Models
{
    public class UserConnection
    {
        public string ConnectionId { get; }

        public string UserName { get; }

        public UserConnection(string userName, string connectionId)
        {
            this.ConnectionId = connectionId;
            this.UserName = userName;
        }
    }
}