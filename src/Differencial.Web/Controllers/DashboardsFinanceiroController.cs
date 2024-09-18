using Differencial.Domain.Contracts.Infra;
using Differencial.Domain.Contracts.Repositories;
using Differencial.Domain.Contracts.Services;
using Differencial.Domain.Util.ExtensionMethods;
using Differencial.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace WEB.Controllers
{

    [Route("Dashboards/FinanceiroReceber")]
    public class DashboardsFinanceiroController : BaseController
    {
        private readonly ISolicitacaoService _solicitacaoService;
        private readonly IDashboardsService _dashboardsService;
        private readonly ISeguradoraService _seguradoraService;
        private readonly IUsuarioService _usuario;
        private readonly ILancamentoFinanceiroTotalRepository _lancamentoFinanceiroTotalRepository;
        IWorkFlowSolicitacaoService _solicitacaoWorkFlowService;

        public DashboardsFinanceiroController(
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

        [HttpGet("")]
        public IActionResult FinanceiroReceber()
        { 
            var selectList = GerarCompetencias(new DateTime(2023, 1, 1), DateTime.Now.AddMonths(1)); 
            ViewData["competenciaMes"] = selectList.ToSelectList(i => i.Key, i => i.Key, false); 
             
            return View();
        }

		[HttpPost("ListarDadosFinanceiroReceber")]		 
        public IActionResult ListarDadosFinanceiroReceber(string mesano)
		{
            var mes = int.Parse(mesano.Split('/')[0]);
            var ano = int.Parse(mesano.Split('/')[1]);


			var lstSolicitacao = _lancamentoFinanceiroTotalRepository.FinanceiroReceber(ano, mes);
            return ResponseResult(true, content: lstSolicitacao);
        }


        [HttpGet("ReceberLancamentos")]
        public ActionResult ReceberLancamentos(int id, int ano, int mes)
        {
            var lstSolicitacao = _lancamentoFinanceiroTotalRepository.FinanceiroLancamentosReceber(id, ano, mes);
            ViewData["NomeSeguradora"] = _seguradoraService.Buscar(id)?.NomeSeguradora;
            ViewData["Mes"] = mes;
            ViewData["Ano"] = ano;
            return View("Financeiro/ReceberLancamentos", lstSolicitacao);
        }

        //public ActionResult FinanceiroPagar()
        //{
        //    var lstSolicitacao = _dashboardsService.ListarSolicitacoesFinanceiro();

        //    return View(lstSolicitacao);
        //}

        //[HttpGet]
        //public ActionResult Faturar(int Id)
        //{
        //    return View("Financeiro/Faturar");
        //}


        private static List<KeyValuePair<string, string>> GerarCompetencias(DateTime dataInicio, DateTime dataFim)
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


    }
}