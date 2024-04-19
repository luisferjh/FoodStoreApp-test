namespace OnlineStoreApp.UseCases.Interfaces
{
    public interface ISenderEmailService
    {
        Task SendEmail(string to, string buyer);
    }
}
