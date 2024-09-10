namespace Differencial.Domain.Contracts.Util
{
    public interface IConfiguracaoEmail
    {
        int? Porta { get; }
        bool HabilitadoSsl { get; }
        string ServidorSmtp { get; }
        string EmailLogon { get; }
        string EmailResposta { get; }
        string EmailSenha { get; }
        string NomeRemetente { get; }
    }
}
