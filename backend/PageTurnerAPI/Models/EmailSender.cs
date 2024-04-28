using System;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using backend.Controllers;
using backend.Interface;

namespace backend.Models
{
    /// <summary>
    /// Modelo para a utilizacao do email
    /// </summary>
    public class EmailSender : IEmailSender
    {
        //Atributos
        private string smtp = "smtp.office365.com";
        private int porta = 587;
        private string mail = "pageturner@outlook.pt";
        private string pw = "ArthurJuliaHugo*2Pedro";

        private readonly PageTurnerContext context;

        public EmailSender(PageTurnerContext _context)
        {
            context = _context;
        }

        /// <summary>
        /// Metodo para enviar email
        /// </summary>
        /// <param name="emailTo"></param>
        /// <param name="subject"></param>
        /// <param name="mensagem"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Metodo para enviar email de confirmacao de registo do utilizador
        /// </summary>
        /// <param name="emailTo">O endereço de email do destinatário.</param>
        /// <param name="idUtilizador">O ID do utilizador.</param>
        /// <returns>Uma tarefa que representa o envio do email de confirmação.</returns>
        public Task SendEmailConfirmationAsync(string emailTo, int idUtilizador)
        {
            string username = Utilizador.GetUsernameById(context, idUtilizador);

            string confirmationEndpoint = "http://localhost:5272/api/Utilizador/ConfirmarEmail/" + idUtilizador; // Ajuste conforme necessário
            string subject = "Confirmação de Email";
            string mensagem = $@"

        Olá {username},

        Obrigado por se registar no PageTurner.

        Para começar a usufruir de todas as funcionalidades, precisamos de confirmar o seu email.

        Ao confirmar o seu email, terá acesso a:
        - Registo de Livros nas estantes.
        - Possibilidade de solicitações de troca a outros utilizadores.

        Por favor, confirme o seu email clicando aqui: {confirmationEndpoint}.

        É importante confirmar o seu email o mais rápido possível para começar a desfrutar de todas as funcionalidades da nossa plataforma.

        Se tiver alguma dúvida ou problema, não hesite em entrar em contacto connosco.

        Obrigado,
        Equipa PageTurner.
    ";

            return SendEmailAsync(emailTo, subject, mensagem);
        }

        /// <summary>
        /// Envio de email para notificar um utilizador sobre a desativação da sua conta.
        /// </summary>
        /// <param name="emailTo">O endereço de e-mail do destinatário.</param>
        /// <returns>Uma tarefa que representa o envio do e-mail de notificação.</returns>
        public async Task SendAccountDeactivationEmailAsync(string emailTo)
        {
            string subject = "Conta Desativada";
            string mensagem = @"
            A sua conta no PageTurner foi desativada conforme solicitado.
            Se precisar de mais informações, entre em contato com pageturner@outlook.pt.
        ";

            await SendEmailAsync(emailTo, subject, mensagem);
        }

        /// <summary>
        /// Envio de email para notificar um utilizador sobre a reativação da sua conta.
        /// </summary>
        /// <param name="emailTo"></param>
        /// <returns></returns>
        public async Task SendAccountReactivationEmailAsync(string emailTo)
        {
            string subject = "Conta Reativada";
            string mensagem = @"
            A sua conta no PageTurner foi reativada.
            Se precisar de mais informações, entre em contato com pageturner@outlook.pt.
        ";

            await SendEmailAsync(emailTo, subject, mensagem);
        }

        /// <summary>
        /// Envio de email para notificar um utilizador sobre o banimento da sua conta.
        /// </summary>
        /// <param name="emailTo">O endereço de e-mail do destinatário.</param>
        /// <returns>Uma tarefa que representa o envio do e-mail de notificação.</returns>
        public async Task SendAccountBanEmailAsync(string emailTo)
        {
            string subject = "Conta Banida";
            string mensagem = @"
            A sua conta no PageTurner foi banida por violação dos termos de uso.
            Se desejar contestar esta decisão, entre em contato com pageturner@outlook.pt.
        ";

            await SendEmailAsync(emailTo, subject, mensagem);
        }

    }
}