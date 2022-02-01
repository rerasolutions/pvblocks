namespace pvblocks_api.Model
{
    public class User
    {
        public string Username { get; set; }
        public JwtToken JwtToken { get; set; } = null!;
    }

    
}