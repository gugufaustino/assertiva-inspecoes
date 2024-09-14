using System;
namespace Differencial.Domain.Contracts.Infra
{
    public interface ILog
    {
        void Registrar(string mensagem, TipoLogEnum tipoLogEnum);
        void Registrar(Exception ex, TipoLogEnum tipoLogEnum);
        void Registrar(Exception ex, string mensagem, TipoLogEnum tipoLogEnum);

    }


    public enum TipoLogEnum
    {
        Erro,
        Informacao,
        RegraNegocio

    }
}


