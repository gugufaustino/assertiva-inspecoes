using Differencial.Domain.Contracts.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace Differencial.Domain.Util
{
    public class ConfiguracaoAplicativo : IConfiguracaoAplicativo, IConfiguracaoEmail
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ConfiguracaoAplicativo(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }
        public string ConnectionString => _configuration.GetConnectionString("DifferencialConnection");
        public string NomeEmpresaCompleto => _configuration.GetValue<string>("NomeEmpresaCompleto");
        public string NomeEmpresaSimples => _configuration.GetValue<string>("NomeEmpresaSimples");
        public string AppVersao => _configuration.GetValue<string>("AppVersao");
        public string UsuarioRoot => _configuration.GetValue<string>("APPVERSAO");
        public string UsuarioRootPwd => _configuration.GetValue<string>("UsuarioRootPwd");
        public string GoogleApiKey => _configuration.GetValue<string>("GoogleApiKey");
        public string PrivateKey => _configuration.GetValue<string>("Private-Key");
        public string DominioAplicativo => $"{_httpContextAccessor.HttpContext?.Request?.Scheme}://{_httpContextAccessor.HttpContext?.Request?.Host.Value}"; 

        #region Diretorios
        private string PastaRepositorio => AppDomain.CurrentDomain.BaseDirectory + _configuration.GetValue<string>("PastaRepositorioGlobal");
        public string RepositorioOperadorImagem => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _configuration.GetValue<string>("PastaOperadorImagem"));
        public string PastaVirtualOperadorImagem => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _configuration.GetValue<string>("PastaVirtualOperadorImagem"));

        public string RepositorioAnexos => Path.Combine(PastaRepositorio, _configuration.GetValue<string>("PastaAnexos"));
        public string RepositorioSolicitacao => Path.Combine(PastaRepositorio, _configuration.GetValue<string>("PastaSolicitacao"));
        public string PastaLog => Path.Combine(PastaRepositorio, _configuration.GetValue<string>("PastaLog"));
        #endregion

        #region Email
        public int? Porta
        {
            get
            {
                var iPorta = 0;
                return int.TryParse(_configuration.GetValue<string>("Porta"), out iPorta) ? iPorta : (int?)null;
            }
        }

        public bool HabilitadoSsl => bool.Parse(_configuration.GetValue<string>("HabilitadoSsl"));

        public string ServidorSmtp => _configuration.GetValue<string>("ServidorSmtp");

        public string EmailLogon => _configuration.GetValue<string>("EmailLogon");

        public string EmailResposta => _configuration.GetValue<string>("EmailResposta");

        public string EmailSenha => _configuration.GetValue<string>("EmailSenha");

        public string NomeRemetente => _configuration.GetValue<string>("NomeRemetente");

       
        #endregion

    }
}
