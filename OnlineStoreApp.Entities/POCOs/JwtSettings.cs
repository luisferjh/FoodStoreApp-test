namespace OnlineStoreApp.Entities.POCOs
{
    public class JwtToken
    {
        public string Token { get; set; }
        public DateTime ExpireTime { get; set; }
        public string RefreshToken { get; set; }
    }
}
