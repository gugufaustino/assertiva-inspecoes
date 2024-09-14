using Differencial.Domain.Contracts.Infra;
using Differencial.Domain.Contracts.Util;
using System;
using System.IO;
namespace Differencial.Infra
{
    internal static class LogStatic
    {
        private const string SEPARADOR = "-----------------------------------------------------------------------------------------";

        internal static void Registrar(string mensagem, TipoLogEnum tipoLogEnum, IConfiguracaoAplicativo configuration)
        {
            
            var nomeArquivo = DateTime.Now.Date.ToString("yyyy-MM-dd") + ".log";
            //var subPasta = "";
            //switch (tipoLogEnum)
            //{
            //    case TipoLogEnum.Erro:
            //        subPasta = "\\Erro\\";
            //        break;
            //    case TipoLogEnum.Informacao:
            //        subPasta = "\\Informacao\\";
            //        break;
            //    case TipoLogEnum.RegraNegocio:
            //        subPasta = "\\RegraNegocio\\";
            //        break;
            //    default:
            //        throw new NotImplementedException();
            //}
            var caminhoPasta = configuration.PastaLog + string.Format("\\{0}\\", tipoLogEnum.ToString());

            if (!Directory.Exists(caminhoPasta))
                Directory.CreateDirectory(caminhoPasta);


            File.AppendAllText(caminhoPasta + nomeArquivo,
                DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + Environment.NewLine
                + mensagem + Environment.NewLine
                + SEPARADOR + Environment.NewLine + Environment.NewLine);
        }
    } 

    public class Log : ILog
    {
        private readonly IConfiguracaoAplicativo configuration;

        public Log(IConfiguracaoAplicativo configuration)
        {
            this.configuration = configuration;            
        }

        public void Registrar(string mensagem, TipoLogEnum tipoLogEnum) => LogStatic.Registrar(mensagem, tipoLogEnum, configuration);

        public void Registrar(Exception ex, TipoLogEnum tipoLogEnum) => Registrar(ex.ToString(), tipoLogEnum);

        public void Registrar(Exception ex, string mensagem, TipoLogEnum tipoLogEnum)
        {
            mensagem = (tipoLogEnum != TipoLogEnum.RegraNegocio ? Environment.NewLine + ex.ToString() : string.Empty);
            Registrar(mensagem, tipoLogEnum);
        }
    }

}


