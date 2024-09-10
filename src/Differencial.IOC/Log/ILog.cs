using System;
namespace Differencial.Infra
{
    public interface ILog
    {
        void Registrar(string mensagem, TipoLogEnum tipoLogEnum);
        void Registrar(Exception ex, TipoLogEnum tipoLogEnum);
        void Registrar(Exception ex, string mensagem, TipoLogEnum tipoLogEnum);

    }
}


