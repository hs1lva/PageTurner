namespace backend.Interface
{
    /// <summary>
    /// Interface para envio de email
    /// </summary>
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string mensagem);
    }
}