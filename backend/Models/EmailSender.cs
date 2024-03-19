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
    /// <summary>
    /// Modelo para a utilizacao do email
    /// </summary>
    public class EmailSender:IEmailSender
    {
        //Atributos
        private string smtp = "smtp.office365.com";
        private int porta = 587;
        private string mail = "pageturner@outlook.pt";
        private string pw = "ArthurJuliaHugo*2Pedro";



        public Task SendEmailAsync(string emailTo, string subject, string mensagem)
        {
            // Lógica para enviar e-mails
            var envio = new SmtpClient(smtp, porta)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(mail, pw)
            };

            return envio.SendMailAsync(
                        new MailMessage(mail, emailTo, subject, mensagem));
        }
    }
    
        
    
}