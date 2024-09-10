using Differencial.Domain.Contracts.Services;
using Differencial.Domain.Contracts.Util;
using System;
using System.Collections.Generic;
using System.Net.Mail;

namespace Differencial.Service.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguracaoEmail _configuracaoEmail;

        public EmailService(IConfiguracaoEmail configuracaoaplicativo)
        {
            _configuracaoEmail = configuracaoaplicativo;
        }

        public void Enviar(string assunto, string corpo, string destinatario)
        {
            Enviar(assunto, corpo, new List<string> { destinatario });
        }

        public void Enviar(String assunto, String corpo, List<String> destinatarios, List<String> destinatariosCopia = null, List<String> destinatariosCopiaOculta = null, List<Attachment> lstAnexos = null)
        {

            MailMessage mail = new();

            foreach (var email in destinatarios)
            {
                mail.To.Add(email);
            }

            if (destinatariosCopia != null)
                foreach (var email in destinatariosCopia)
                {
                    mail.CC.Add(email);
                }
            if (destinatariosCopiaOculta != null)
                foreach (var email in destinatariosCopiaOculta)
                {
                    mail.Bcc.Add(email);
                }

            if (lstAnexos != null)
                foreach (var anexo in lstAnexos)
                {
                    mail.Attachments.Add(anexo);
                }


            mail.From = new MailAddress(_configuracaoEmail.EmailLogon, _configuracaoEmail.NomeRemetente, System.Text.Encoding.UTF8);
            mail.Subject = assunto;
            mail.Body = corpo;
            mail.SubjectEncoding = System.Text.Encoding.UTF8;
            mail.BodyEncoding = System.Text.Encoding.UTF8;
            mail.IsBodyHtml = true;

            mail.ReplyToList.Add(new MailAddress(_configuracaoEmail.EmailResposta));
            SmtpClient client = new()
            {
                Credentials = new System.Net.NetworkCredential(_configuracaoEmail.EmailLogon, _configuracaoEmail.EmailSenha),
                EnableSsl = _configuracaoEmail.HabilitadoSsl,
                Host = _configuracaoEmail.ServidorSmtp
            };

            if (_configuracaoEmail.Porta.HasValue)
                client.Port = _configuracaoEmail.Porta.Value;


            client.Send(mail);



        }
    }
}



