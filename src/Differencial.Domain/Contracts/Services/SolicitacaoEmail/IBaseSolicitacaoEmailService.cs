using Differencial.Domain.Entities;
using System.IO;

namespace Differencial.Domain.Contracts.Services.SolicitacaoEmail
{
    public interface IBaseSolicitacaoEmailService<T1>
        where T1 : class
        
    {
        /// <summary>
        /// Metodo para validar se o email é da seguradora e se email de agendamento está aberto
        /// </summary> 
        void ValidarEmail();

        /// <summary>
        /// Metodo para verificar se já foi cadastrodo solicitação para mesmo pedido da seguradora;
        /// </summary>
        /// <param name="CodSolicitacaoSeguradora"></param>
        void ValidarExisteSolicitacao(int CodSolicitacaoSeguradora);

        FileStream BaixarSolicitacaoPDF(string htmlBody);

        FileStream BaixarEmailSolicitacaoPDF(string htmlBody);
 
        Endereco EnderecoClienteSolicitacao();

        Solicitacao NovaSolicitacao();

    }
}
