using Differencial.Domain.DTO;
using Differencial.Domain.Entities;
using Differencial.Domain.Exceptions;
using Differencial.Domain.Util.ExtensionMethods;
using Differencial.Domain.Validation;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace Differencial.Domain.Contracts.Services.SolicitacaoEmail
{
    public class SolicitacaoEmailSompo : ISompoSolicitacaoEmailService
    {

        private readonly EmailModelDTO _emailModel;
        private readonly ISolicitacaoService _solicitacaoService;
        private readonly ISeguradoraService _seguradoraService;
        private readonly IProdutoService _produtoService;
        private readonly IClienteService _clienteService;
        private readonly string _htmlRequest;
        private readonly string _linkSoliAgendaInspecao;

        public SolicitacaoEmailSompo(EmailModelDTO emailModel,
            ISolicitacaoService solicitacaoService,
            ISeguradoraService seguradoraService,
            IClienteService clienteService,
            IProdutoService produtoService)
        {
            _emailModel = emailModel;
            _solicitacaoService = solicitacaoService;
            _seguradoraService = seguradoraService;
            _clienteService = clienteService;
            _produtoService = produtoService;


            _linkSoliAgendaInspecao = UrlSolicitacaoSistema(_emailModel.corpoHtml);
            _htmlRequest = RequestSiteSeguradora(_linkSoliAgendaInspecao);
            ValidarEmail();

        }

        public FileStream BaixarEmailSolicitacaoPDF(string htmlBody)
        {
            throw new NotImplementedException();
        }

        public FileStream BaixarSolicitacaoPDF(string htmlBody)
        {
            throw new NotImplementedException();
        }

        public void ValidarEmail()
        {
            var validationResultsManager = new ValidationResultsManager();

            if (_htmlRequest.IndexOf("name=\"action\" value=\"consultar\"") > 0)
                validationResultsManager.AddValidationResultNotValid("Agenda já foi realizada.");

            //TODO Ver outra situação onde não é possível abrir a partir do link
            //lstValidation.Add(new BusinessValidationResult(false, "Não foi possível abrir a solicitação no site da compania.")); 

            if (validationResultsManager.HasError)
                validationResultsManager.ThrowException("Esse e-mail não é válido para essa operação, não sendo possível criar a solicitação.");

        }

        private string UrlSolicitacaoSistema(string html)
        {
            var paramIdenCod = string.Empty;

            if (StringHelper.TryValorEntreCaracter(_emailModel.corpoHtml, "codIdenDoc=", "\'", out paramIdenCod) == false && StringHelper.TryValorEntreCaracter(_emailModel.corpoHtml, "codIdenDoc=", "\"", out paramIdenCod) == false)
            {
                throw new ValidationException("Esse e-mail não é válido para criar a solicitação." + Environment.NewLine + " Não há link para consulta ao sistema SOMPO");
            }
            return string.Format("http://www.yasuda.com.br/AgendaInspecao/Controller?action=Inspecao&codIdenDoc={0}", paramIdenCod);
        }

        public Solicitacao NovaSolicitacao()
        {
            int CodInspecao = 0;
            int.TryParse(ValorProximaCelula(_htmlRequest, "Inspe&ccedil;&atilde;o N&ordm;:"), out CodInspecao);

            int CodProdutoSeguradora = 0;
            int.TryParse(ValorProximaCelula(_htmlRequest, "Ramo").Trim().RemoveNonNumbers(), out CodProdutoSeguradora);
            var produto = _produtoService.BuscarPorCodProdutoSeguradora(CodProdutoSeguradora.ToString());

            var solic = new Solicitacao()
            {
                CodSeguradora = CodInspecao.ToString(),
                Produto = produto,
                IdProduto = produto != null ? produto.Id : 0,
                CorretorNome = ValorProximaCelula(_htmlRequest, "Corretor"),
                CorretorTelefone = ValorProximaCelula(_htmlRequest, "Telefone Corretor").Replace("(00", "(").TrataTelefone(),
                TxtInformacoesAdicionais = ValorProximaCelula(_htmlRequest, "Tipo de Cobertura"),
            };

            return solic;
        }

        public void ValidarExisteSolicitacao(int CodSolicitacaoSeguradora)
        {
            throw new NotImplementedException();
        }

        public Endereco EnderecoClienteSolicitacao()
        {
            var endereco = new Endereco();

            endereco.Bairro = ValorProximaCelula(_htmlRequest, "Bairro");
            endereco.Cep = ValorProximaCelula(_htmlRequest, "CEP");

            endereco.Logradouro = ValorProximaCelula(_htmlRequest, "Endere&ccedil;o:").TrataLogradouro();
            endereco.Complemento = ValorProximaCelula(_htmlRequest, "Endere&ccedil;o:").TrataComplemento();
            endereco.Numero = ValorProximaCelula(_htmlRequest, "Endere&ccedil;o:").TrataNumero();

            endereco.NomeMunicipio = ValorProximaCelula(_htmlRequest, "Cidade");
            endereco.SiglaUf = ValorProximaCelula(_htmlRequest, "UF");

            return endereco;
        }

        public Cliente ClienteSolicitacao()
        {
            var cli = new Cliente();

            cli.NomeRazaoSocial = ValorProximaCelula(_htmlRequest, "Proponente");

            cli.CpfCnpj = ValorProximaCelula(_htmlRequest, "CNPJ");

            cli.ContatoNome = ValorProximaCelula(_htmlRequest, "Contato 1");
            cli.ContatoTelefone = ValorProximaCelula(_htmlRequest, "Telefone Contato 1").TrataTelefone();

            cli.ContatoOutro = ValorProximaCelula(_htmlRequest, "Contato 2");
            cli.ContatoOutro += " " + ValorProximaCelula(_htmlRequest, "Telefone Contato 2").TrataTelefone();

            cli.ContatoOutro += " " + ValorProximaCelula(_htmlRequest, "E-mail de Contato");

            cli.AtividadeNome = ValorProximaCelula(_htmlRequest, "Atividade Principal");

            return cli;
        }

        private string RequestSiteSeguradora(string url)
        {
            // Create a request for the URL. 
            WebRequest request = WebRequest.Create(url);
            // If required by the server, set the credentials.
            request.Credentials = CredentialCache.DefaultCredentials;
            WebResponse response = request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream, Encoding.GetEncoding("ISO-8859-1"));
            // Read the content.
            var _html = reader.ReadToEnd();
            // Display the content.
            // Clean up the streams and the response.
            reader.Close();
            response.Close();

            return _html;
        }

        #region "Metodos de auxiliares -  manipulação de string para interpretar html de email"
         
        private string ValorProximaCelula(string html, string LabelCell)
        {
            //<td width="183" class="boxTexto">Proponente:</td>
            //<td colspan="2" class="boxTexto" width="369">IRMAOS LAZARIN LTDA</td>
            if (html.IndexOf(LabelCell) > -1)
            {
                var tmp = html.Substring(html.IndexOf(LabelCell) + LabelCell.Length);
                tmp = tmp.Substring(tmp.IndexOf("\">") + "\">".Length);

                if (tmp.IndexOf("</td>") >= 0)
                    tmp = tmp.Substring(0, tmp.IndexOf("</td>"));

                return tmp.Replace("\r", string.Empty).Replace("\n", string.Empty).Replace("\t", string.Empty).Trim();
            }
            else
                return "";

        }

        public static class StringHelper
        {
            public static bool TryValorEntreCaracter(string texto, string caracterAbre, string caracterFecha, out string valorRetorno)
            {
                try
                {
                    valorRetorno = texto.ValorEntreCaracter(caracterAbre, caracterFecha);
                    return true;
                }
                catch (Exception ex)
                {
                    valorRetorno = ex.Message;
                }
                return false;
            }
        }
        #endregion


    }
    internal static class TratamentoDados
    {
        public static string TrataLogradouro(this string valor)
        {
            var arrLog = valor.Split(',');
            if (arrLog.Length >= 0)
            {
                valor = arrLog[0].Trim();
            }

            //REMOVER DADOS INVÁLIDOS:
            if (valor.ToUpper().IndexOf("R RUA") > -1)
                valor = valor.Replace("R RUA", "Rua");

            if (valor.ToUpper().IndexOf("ROD ") > -1)
                valor = valor.Replace("ROD ", "");


            return valor;
        }
        public static string TrataComplemento(this string valor)
        {
            if (valor.Contains(" - "))
            {
                return valor.Substring(valor.IndexOf(" - ") + " - ".Length);
            }
            return string.Empty;
        }

        public static int? TrataNumero(this string valor)
        {
            var strComplemento = string.Empty;
            if (valor.Contains(" - "))
            {
                strComplemento = valor.Substring(valor.IndexOf(" - "));
            }

            var arrLog = valor.Split(',');
            if (arrLog.Length > 0)
            {

                if (strComplemento.Length > 0)
                    valor = arrLog[1].Replace(strComplemento, string.Empty).Trim();
                else
                    valor = arrLog[1].Trim();
            }


            int ival = 0;
            if (int.TryParse(valor, out ival) && ival > 0)
                return ival;
            else
                return null;


        }

        public static string TrataTelefone(this string valor)
        {
            //REMOVER DADOS INVÁLIDOS:

            valor = valor.Replace(" ", string.Empty).Replace("-", string.Empty);
            return valor;
        }
    }
}
