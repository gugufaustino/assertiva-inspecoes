using Differencial.Domain;
using Differencial.Domain.Contracts.Infra;
using Differencial.Domain.Contracts.Repositories;
using Differencial.Domain.Contracts.Services;
using Differencial.Domain.Entities;
using Differencial.Domain.Filters;
using Differencial.Domain.Resources;
using Differencial.Web.Controllers;
using Differencial.Web.Filters;
using Differencial.Web.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Differencial.Domain.Util.ExtensionMethods;
using Differencial.Domain.UOW;
using System;

namespace WEB.Controllers
{
    public class DashboardsController : BaseController
    {
        private readonly ISolicitacaoService _solicitacaoService;
        private readonly IDashboardsService _dashboardsService;
        private readonly ISeguradoraService _seguradoraService;
        private readonly IUsuarioService _usuario;
        private readonly ILancamentoFinanceiroTotalRepository _lancamentoFinanceiroTotalRepository;
        IWorkFlowSolicitacaoService _solicitacaoWorkFlowService;

        public DashboardsController(
            ISolicitacaoService solicitacaoService,
            IDashboardsService dashboardsService,
            ISeguradoraService seguradoraService,
            IUsuarioService usuario,
            ILancamentoFinanceiroTotalRepository lancamentoFinanceiroTotalRepository,
            IWorkFlowSolicitacaoService workFlowSolicitacaoService)
        {
            _solicitacaoService = solicitacaoService;
            _dashboardsService = dashboardsService;
            _seguradoraService = seguradoraService;
            _usuario = usuario;
            _lancamentoFinanceiroTotalRepository = lancamentoFinanceiroTotalRepository;
            _solicitacaoWorkFlowService = workFlowSolicitacaoService;
        }
        public ActionResult DashboardSeguradora()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Editar(int id)
        {
            return RedirectToAction("Editar", "Solicitacao", new { id = id });

        }
        public ActionResult Gerente()
        { 
            var lstSeguradoras = _seguradoraService.Listar(new SeguradoraFilter { IndAtivo = true, CampoOrdenacao = CampoOrdenacaoSeguradora.NomeSeguradora }).ToList(); 

            var segIndSolicEmail = lstSeguradoras.FirstOrDefault(i=>i.IndIntegracaoSolicitacaoPorEmail ==  true );

            ViewBag.lstSeguradoras = lstSeguradoras; 
            ViewBag.gmailSeguradoraEmailRemetente = segIndSolicEmail != null ? segIndSolicEmail.EmailRemetenteSolicitacao : string.Empty;
            ViewBag.gmailCredencialAplicativo = "28124896797-7h0hvq5m3mm35dnn5qsps8ibg8brs05q.apps.googleusercontent.com";
            ViewBag.gmailIndIntegracaoEmailAbilitada = true;


            var lstSolicitacao = _dashboardsService.ListarSolicitacoesGerencia();
            ViewBag.lstSolicAgenda = _dashboardsService.ListarSolicitacoesGerenciaAgendamento();

            return View(lstSolicitacao);
        }

        public ActionResult Vistoriador()
        {
            var lstSolicitacao = _dashboardsService.ListarSolicitacoesVistoriador();
            return View(lstSolicitacao);
        }

        public ActionResult Analista()
        {
            ViewBag.ListaMinhasTerefas = _dashboardsService.ListarSolicitacoesAnalistaMinhas();
            ViewBag.ListaAnaliseTerefas = _dashboardsService.ListarSolicitacoesAnalista();

            return View();
        }

        public ActionResult Solicitante()
        {
            var lstSolicitacao = _dashboardsService.ListarSolicitacoesSolicitante();

            return View(lstSolicitacao);
        }
        
        public ActionResult FinanceiroReceber(int ano, int mes)
        {

            var selectList = GerarCompetencias(new DateTime(2023, 1, 1), DateTime.Now);

            ViewData["competenciaMes"] = selectList.ToSelectList(i => i.Key, i => i.Key, false);

            var lstSolicitacao = _lancamentoFinanceiroTotalRepository.FinanceiroReceber(ano, mes);

            return View(lstSolicitacao);
        }

        public static List<KeyValuePair<string, string>> GerarCompetencias(DateTime dataInicio, DateTime dataFim)
        {
            var listaCompetencias = new List<KeyValuePair<string, string>>();

            var dataAtual = dataFim;

            while (dataAtual >= dataInicio)
            {
                var key = $"{dataAtual.Month}/{dataAtual.Year}";
                var value = $"{dataAtual.ToString("MMMM", new System.Globalization.CultureInfo("pt-BR"))}/{dataAtual.Year}";

                listaCompetencias.Add(new KeyValuePair<string, string>(key, value));

                dataAtual = dataAtual.AddMonths(-1);
            }

            return listaCompetencias;
        }

        [HttpGet]
        public ActionResult ReceberLancamentos(int id, int ano, int mes)
        {
            var lstSolicitacao = _lancamentoFinanceiroTotalRepository.FinanceiroLancamentosReceber(id, ano, mes);
            ViewData["NomeSeguradora"] = _seguradoraService.Buscar(id)?.NomeSeguradora;
            ViewData["Mes"] = mes;
            ViewData["Ano"] = ano;
            return View("Financeiro/ReceberLancamentos", lstSolicitacao);
        }
        
        public ActionResult FinanceiroPagar()
        {
            var lstSolicitacao = _dashboardsService.ListarSolicitacoesFinanceiro();

            return View(lstSolicitacao);
        }

        [HttpGet]
        public ActionResult Faturar(int Id)
        {
            return View("Financeiro/Faturar");
        }

        [HttpPost]
        [ServiceFilter(typeof(TransactionFilter))]
        public JsonResult Excluir(int[] Id)
        {

            _solicitacaoService.Excluir( Id);
            Commit();
            var lstSolicitacao = _dashboardsService.ListarSolicitacoesGerencia();
            var result = MontarListaSolicitacaoGerente(lstSolicitacao);
            return ResponseResult(true, content: result, message: MensagensSucesso.SucessoExcluir);

        }

        [HttpPost]
        [ServiceFilter(typeof(TransactionFilter))]
        public JsonResult ApropriarSolicitacao(int Id)
        {
            _solicitacaoWorkFlowService.Apropriar(Id);
            Commit();
            var lstSolicitacao = _dashboardsService.ListarSolicitacoesAnalista();
            var result = MontarListaSolicitacaoAnalise(lstSolicitacao);
            return ResponseResult(true, content: result);
        }

        public JsonResult AtualizarSolicitacao()
        {

            var lstSolicitacao = _dashboardsService.ListarSolicitacoesAnalistaMinhas();
            var result = MontarListaSolicitacaoAnaliseMinhas(lstSolicitacao);
            return ResponseResult(true, content: result);
        }


        private IEnumerable<object> MontarListaSolicitacaoGerente(IEnumerable<Solicitacao> lstEntidade)
        {

            var lst = lstEntidade.Select(item =>
                    new
                    {
                        item.Id,
                        item.CodSeguradora,
                        NomeSeguradora = item.Seguradora.NomeSeguradora,
                        item.Produto.NomeProduto,
                        NomeMunicipio = HtmlGridHelper.TextoSubTexto(item.Cliente.NomeRazaoSocial, item.Endereco.NomeMunicipio + " - " + item.Endereco.SiglaUf).ToString(),
                        TpSituacao = HtmlGridHelper.SituacaoProcesso(item.TpSituacao).ToString(),
                        DataCadastro = item.DataCadastro.ToString(),
                        DataModificacao = item.DataModificacao.ToString(),
                        DefinirVistoriador = HtmlGridHelper.SituacaoAtividade(item.AtividadeProcesso, TipoAtividadeEnum.DefinirVistoriador).ToString(),
                        DefinirAnalista = HtmlGridHelper.SituacaoAtividade(item.AtividadeProcesso, TipoAtividadeEnum.DefinirAnalista).ToString(),
                        IndUrgente = item.IndUrgente,
                    });

            return lst;
        }
        private IEnumerable<object> MontarListaSolicitacaoAnalise(IEnumerable<Solicitacao> lstEntidade)
        {

            var lst = lstEntidade.Select(item =>
                    new
                    {
                        item.Id,
                        item.CodSeguradora,
                        SeguradoraProduto = HtmlGridHelper.TextoSubTexto(item.Produto.Seguradora.NomeSeguradora, item.Produto.NomeProduto).ToString(),
                        ClienteRazaoSocial = HtmlGridHelper.TextoSubTexto(item.Cliente.NomeRazaoSocial, item.Endereco + "- " + item.Endereco.SiglaUf).ToString(),
                        SituacaoProcesso = HtmlGridHelper.SituacaoProcesso(item.TpSituacao).ToString(),
                        OperadorApropriado = item.OperadorApropriado.NomeOperador.ToString(),
                        DataApropriado = item.MovimentacaoProcesso.LastOrDefault(m => m.TipoSituacaoProcesso == TipoSituacaoProcessoEnum.ApropriadoPelaAnalise).DthMovimentacao.ToString(),
                        DataEnviado = item.MovimentacaoProcesso.LastOrDefault(m => m.TipoSituacaoProcesso == TipoSituacaoProcessoEnum.ApropriadoPelaAnalise).DthMovimentacao.ToString(),
                        ElaborarCroquiAnalise = HtmlGridHelper.SituacaoAtividade(item.AtividadeProcesso, TipoAtividadeEnum.ElaborarCroquiAnalise).ToString(),
                        ElaborarQuadro = HtmlGridHelper.SituacaoAtividade(item.AtividadeProcesso, TipoAtividadeEnum.ElaborarCroquiAnalise).ToString(),
                        ElaborarEnviarLaudo = HtmlGridHelper.SituacaoAtividade(item.AtividadeProcesso, TipoAtividadeEnum.ElaborarCroquiAnalise).ToString(),
                        NomeMunicipio = HtmlGridHelper.TextoSubTexto(item.Cliente.NomeRazaoSocial, item.Endereco.NomeMunicipio + " - " + item.Endereco.SiglaUf).ToString(),
                        TempoDecorrido = @HtmlGridHelper.TempoDecorrido(item.MovimentacaoProcesso.LastOrDefault(m => m.TipoSituacaoProcesso == TipoSituacaoProcessoEnum.ApropriadoPelaAnalise).DthMovimentacao).ToString(),
                        IndUrgente = item.IndUrgente,
                        IndCidadeBaseVistoriador = item.IndCidadeBaseVistoriador.ToString(),
                        NomeMunicipioSiglaUf = item.NomeMunicipioSiglaUf.ToString()
                    });

            return lst;
        }

        private IEnumerable<object> MontarListaSolicitacaoAnaliseMinhas(IEnumerable<Solicitacao> lstEntidade)
        {

            var lst = lstEntidade.Select(item =>
                    new
                    {
                        item.Id,
                        item.CodSeguradora,
                        SeguradoraProduto = HtmlGridHelper.TextoSubTexto(item.Produto.Seguradora.NomeSeguradora, item.Produto.NomeProduto).ToString(),
                        ClienteRazaoSocial = HtmlGridHelper.TextoSubTexto(item.Cliente.NomeRazaoSocial, item.Endereco + "- " + item.Endereco.SiglaUf).ToString(),
                        DataApropriado = item.MovimentacaoProcesso.LastOrDefault(m => m.TipoSituacaoProcesso == TipoSituacaoProcessoEnum.ApropriadoPelaAnalise).DthMovimentacao.ToString(),
                        ElaborarCroquiAnalise = HtmlGridHelper.SituacaoAtividade(item.AtividadeProcesso, TipoAtividadeEnum.ElaborarCroquiAnalise).ToString(),
                        ElaborarQuadro = HtmlGridHelper.SituacaoAtividade(item.AtividadeProcesso, TipoAtividadeEnum.ElaborarCroquiAnalise).ToString(),
                        ElaborarEnviarLaudo = HtmlGridHelper.SituacaoAtividade(item.AtividadeProcesso, TipoAtividadeEnum.ElaborarCroquiAnalise).ToString(),
                        TempoDecorrido = @HtmlGridHelper.TempoDecorrido(item.MovimentacaoProcesso.LastOrDefault(m => m.TipoSituacaoProcesso == TipoSituacaoProcessoEnum.ApropriadoPelaAnalise).DthMovimentacao).ToString(),
                        IndUrgente = item.IndUrgente,
                    });

            return lst;
        }
    }
}