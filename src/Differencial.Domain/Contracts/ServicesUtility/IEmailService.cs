using System;
using System.Collections.Generic;
using System.Net.Mail;

namespace Differencial.Domain.Contracts.Services
{
    public interface IEmailService
	{
		void Enviar(String assunto, String corpo, List<string> destinatarios, List<String> destinatariosCopia = null, List<String> destinatariosCopiaOculta = null, List<Attachment> lstAnexos = null);
        void Enviar(String assunto, string corpo, string destinatario);
 
	}
}