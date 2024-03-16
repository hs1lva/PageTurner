namespace backend.Interface
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string mensagem);
    }
}