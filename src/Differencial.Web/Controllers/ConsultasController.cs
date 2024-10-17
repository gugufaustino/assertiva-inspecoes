using Differencial.Domain;
using Differencial.Domain.Contracts.Infra;
using Differencial.Domain.Contracts.Repositories;
using Differencial.Domain.Contracts.Services;
using Differencial.Domain.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Differencial.Web.Controllers
{
    public class ConsultasController : BaseController
    {
        IConsultasService _consultasService;
        ILancamentoFinanceiroTotalService _lancamentoFinanceiroTotalService;
		ISolicitacaoRepository _solicitacaoRepositorio;
		public ConsultasController(IConsultasService solicitacaoService,
            ILancamentoFinanceiroService lancamentosFinanceitoService,
            ILancamentoFinanceiroTotalService  lancamentoFinanceiroTotalService,
			ISolicitacaoRepository solicitacaoRepositorio)
        {
            _consultasService = solicitacaoService;            
            _lancamentoFinanceiroTotalService = lancamentoFinanceiroTotalService;

			_solicitacaoRepositorio = solicitacaoRepositorio;

		}
        public ActionResult TodasSolicitacoes()
        {
            var lstSolic = _consultasService.ListarTodasSolicitacoes();
            return View(lstSolic);
        }

        public ActionResult TodasSolicitacoesConcluidas()
        {
            var lstSolic = _consultasService.ListarConcluidas();
            return View(lstSolic);
        }

        public ActionResult TodasSolicitacoesTramitacao()
        {
            var lstSolic = _consultasService.ListarEmTramitacao();
            return View(lstSolic);
        }
        public ActionResult TodasAgendas()
        {
            var lstSolic = _consultasService.ListarTodasAgendas();
            return View(lstSolic);
        }
        public async Task<ActionResult> TodasRotas()
        {
            var lstSolic = await _solicitacaoRepositorio.ListarTodasRotas(new SolicitacaoFilter()); 

            return View(lstSolic);
        }
        public ActionResult TodosLancamentosFinanceiros()
        {
            var lst = _lancamentoFinanceiroTotalService.TodosLancamentosFinanceiros();
            return View(lst);
        }
    }
}