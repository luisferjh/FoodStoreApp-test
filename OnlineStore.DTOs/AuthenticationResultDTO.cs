namespace OnlineStore.DTOs
{
    public class AuthenticationResultDTO
    {
        public AuthenticationResultDTO()
        {
            Errors = new List<string>();
        }
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
        public object Result { get; set; }
    }
}
