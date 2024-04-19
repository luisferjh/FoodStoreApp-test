namespace OnlineStoreApp.UseCases.Helpers
{
    public class JwtSettings
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string SecretKey { get; set; }
        public TimeSpan TokenLifeTime { get; set; }
    }
}
