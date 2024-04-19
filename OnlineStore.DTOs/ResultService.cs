namespace OnlineStore.DTOs
{
    public class ResultService
    {
        public bool IsSuccess { get; set; }
        public object Data { get; set; }
        public string ErrorMessage { get; set; }
        public string Message { get; set; }
    }
}
