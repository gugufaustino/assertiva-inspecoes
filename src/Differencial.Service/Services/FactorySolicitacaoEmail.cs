using Differencial.Domain.Contracts.Services;
using Differencial.Domain.Contracts.Services.SolicitacaoEmail;
using Differencial.Domain.DTO;
using Differencial.Domain.Entities;
using Differencial.Domain.Exceptions;
using Differencial.Domain.Resources;
using Differencial.Domain.Util.ExtensionMethods;

namespace Differencial.Service.Services
{

    public class FactorySolicitacaoEmail
    {
        ISolicitacaoService _solicitacaoService;
        ISeguradoraService _seguradoraService;
        IClienteService _clienteService;
        IProdutoService _produtoService;

        private FactorySolicitacaoEmail() {
        }

        public FactorySolicitacaoEmail(ISolicitacaoService solicitacaoService,
                                        ISeguradoraService seguradoraService,
                                        IClienteService clienteService,
                                        IProdutoService produtoService)
        {
            _solicitacaoService = solicitacaoService;
            _seguradoraService = seguradoraService;
           _clienteService = clienteService;
            _produtoService = produtoService;
           
        }

        public Solicitacao Criar(EmailModelDTO emailModel)
        {
            var seguradora = _seguradoraService.ObterPorRemetenteSolicitacao(emailModel.remetente.ValorEntreCaracter("<", ">"));

            if (seguradora == null)
                throw new ValidationException(MensagensValidacaoServicos.RnFactorySolicitacaoEmail_RemetenteNaoEncontrado.Formata(emailModel.remetente));

            if (!seguradora.IndIntegracaoSolicitacaoPorEmail)
                throw new ValidationException(MensagensValidacaoServicos.RnFactorySolicitacaoEmail_SeguradoraNaoSolicita.Formata(seguradora.NomeSeguradora, "'Solicitar por e-mail'"));

            if (seguradora.NomeSeguradora.ToLower().IndexOf("sompo") < 0)
                throw new ValidationException(MensagensValidacaoServicos.RnFactorySolicitacaoEmail_NaoImplementada.Formata(seguradora.NomeSeguradora));
             
            SolicitacaoEmailSompo service = new SolicitacaoEmailSompo(emailModel, _solicitacaoService, _seguradoraService, _clienteService, _produtoService);
             
            Solicitacao solicitacao = service.NovaSolicitacao();
            solicitacao.Endereco = service.EnderecoClienteSolicitacao();            
            solicitacao.Cliente = service.ClienteSolicitacao();
            solicitacao.Seguradora = seguradora;
            solicitacao.IdSeguradora = seguradora.Id;
            return solicitacao;
        }
    }


}
