using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
namespace Differencial.Infra
{
    internal static class LogStatic
    {
        private const string SEPARADOR = "-----------------------------------------------------------------------------------------";

        internal static void Registrar(string mensagem, TipoLogEnum tipoLogEnum, IConfiguration configuration)
        {
            var config = new Domain.Util.ConfiguracaoAplicativo(configuration);
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
            var caminhoPasta = config.PastaLog + string.Format("\\{0}\\", tipoLogEnum.ToString());

            if (!Directory.Exists(caminhoPasta))
                Directory.CreateDirectory(caminhoPasta);


            File.AppendAllText(caminhoPasta + nomeArquivo,
                DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + Environment.NewLine
                + mensagem + Environment.NewLine
                + SEPARADOR + Environment.NewLine + Environment.NewLine);
        }
    }

    public enum TipoLogEnum
    {
        Erro,
        Informacao,
        RegraNegocio

    }


    public class Log : ILog
    {
        private readonly IConfiguration configuration;

        public Log(IConfiguration configuration)
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


