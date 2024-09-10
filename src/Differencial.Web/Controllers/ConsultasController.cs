using Differencial.Domain;
using Differencial.Domain.Contracts.Infra;
using Differencial.Domain.Contracts.Services;
using Differencial.Domain.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Differencial.Web.Controllers
{
    public class ConsultasController : BaseController
    {
        IConsultasService _consultasService;
        ILancamentoFinanceiroTotalService _lancamentoFinanceiroTotalService;
        public ConsultasController(IConsultasService solicitacaoService,
            ILancamentoFinanceiroService lancamentosFinanceitoService,
            ILancamentoFinanceiroTotalService  lancamentoFinanceiroTotalService)
        {
            _consultasService = solicitacaoService;            
            _lancamentoFinanceiroTotalService = lancamentoFinanceiroTotalService;
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
        public ActionResult TodasRotas()
        {
            var lstSolic = _consultasService.Listar(new SolicitacaoFilter());

         lstSolic  = lstSolic.Where(w=> w.IdVistoriador != null && w.AtividadeProcesso.Any(a=>a.TipoAtividade == TipoAtividadeEnum.PrestacaoContaKm))
                                    .OrderBy(o => o.Vistoriador.Operador.NomeOperador);

            return View(lstSolic);
        }
        public ActionResult TodosLancamentosFinanceiros()
        {
            var lst = _lancamentoFinanceiroTotalService.TodosLancamentosFinanceiros();
            return View(lst);
        }
    }
}