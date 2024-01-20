namespace API.Models
{
    public class Token
    {
        public string AccessToken { get; set; }

        public DateTime ExpiresIn { get; set; }
    }
}
