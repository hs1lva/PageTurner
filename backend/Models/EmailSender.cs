using System;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using backend.Interface;

namespace backend.Models
{
    public class EmailSender:IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string mensagem)
        {
            var mail = "pageturner@outlook.pt";
            var pw = "ArthurJuliaHugo*2Pedro";

            // Lógica para enviar e-mails

            var cliente = new SmtpClient("smtp.office365.com", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(mail, pw)
            };

            return cliente.SendMailAsync(
                        new MailMessage(mail, email, subject, mensagem));
        }
    }
    
        
    
}