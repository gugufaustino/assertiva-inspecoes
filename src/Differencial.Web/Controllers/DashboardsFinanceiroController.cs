using Differencial.Domain.Contracts.Repositories;
using Differencial.Domain.Contracts.Services;
using Differencial.Domain.Entities;
using Differencial.Domain.UOW;
using Differencial.Domain.Util.ExtensionMethods;
using Differencial.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WEB.Controllers
{

    [Route("Dashboards")]
	public class DashboardsFinanceiroController : BaseController
	{ 
		private readonly ISeguradoraService _seguradoraService; 
		private readonly ILancamentoFinanceiroTotalRepository _lancamentoFinanceiroTotalRepository;
		private readonly ILancamentoFinanceiroTotalService _lancamentoFinanceiroTotalService;
		private readonly IOperadorService _operadorService;

		public DashboardsFinanceiroController( 
			ISeguradoraService seguradoraService, 
			ILancamentoFinanceiroTotalRepository lancamentoFinanceiroTotalRepository, 
			ILancamentoFinanceiroTotalService lancamentoFinanceiroTotalService,
			IOperadorService operadorService)
		{
			 
			_seguradoraService = seguradoraService; 
			_lancamentoFinanceiroTotalRepository = lancamentoFinanceiroTotalRepository;
			_lancamentoFinanceiroTotalService = lancamentoFinanceiroTotalService;
			_operadorService = operadorService;
		}

        #region FinanceiroReceber
        [HttpGet("FinanceiroReceber")]
		public IActionResult FinanceiroReceber()
		{
			var selectList = GerarCompetencias(new DateTime(2023, 1, 1), DateTime.Now.AddMonths(1));
			ViewData["competenciaMes"] = selectList.ToSelectList(i => i.Key, i => i.Key, false);

			return View();
		}

        [HttpPost("FinanceiroReceber/ListarDadosFinanceiroReceber")]
		public IActionResult ListarDadosFinanceiroReceber(string mesano)
		{
			var mes = int.Parse(mesano.Split('/')[0]);
			var ano = int.Parse(mesano.Split('/')[1]); 

			var lstSolicitacao = _lancamentoFinanceiroTotalRepository.FinanceiroReceber(ano, mes);
			return ResponseResult(true, content: lstSolicitacao);
		}

        [HttpGet("FinanceiroReceber/ReceberLancamentos")]
		public ActionResult ReceberLancamentos(int id, string mesano)
		{
			var mes = int.Parse(mesano.Split('/')[0]);
			var ano = int.Parse(mesano.Split('/')[1]);

			var lstSolicitacao = _lancamentoFinanceiroTotalRepository.FinanceiroLancamentosReceber(id, ano, mes);
			ViewData["NomeSeguradora"] = _seguradoraService.Buscar(id)?.NomeSeguradora;
			ViewData["Mes"] = mes;
			ViewData["Ano"] = ano;
			return View("ReceberLancamentos", lstSolicitacao);
		}



        [HttpPost("FinanceiroReceber/Faturar")]
		[ServiceFilter(typeof(TransactionFilter))]
		public async Task<JsonResult> Faturar(int[] Id, string mesano)
		{
			var mes = int.Parse(mesano.Split('/')[0]);
			var ano = int.Parse(mesano.Split('/')[1]);

			await _lancamentoFinanceiroTotalService.Faturar(Id, ano, mes);
			await AppSaveChangesAsync();
			return ResponseResult(true);
		}

        [HttpPost("FinanceiroReceber/Liquidar")]
		[ServiceFilter(typeof(TransactionFilter))]
		public async Task<JsonResult> Liquidar(int[] Id, string mesano)
		{
			var mes = int.Parse(mesano.Split('/')[0]);
			var ano = int.Parse(mesano.Split('/')[1]);

			await _lancamentoFinanceiroTotalService.Liquidar(Id, ano, mes);
			await AppSaveChangesAsync();
			return ResponseResult(true);
		}

        #endregion


        #region FinanceiroPagar
        [HttpGet("FinanceiroPagar")]
        public IActionResult FinanceiroPagar()
        {
            var selectList = GerarCompetencias(new DateTime(2023, 1, 1), DateTime.Now.AddMonths(1));
            ViewData["competenciaMes"] = selectList.ToSelectList(i => i.Key, i => i.Key, false);
           
            return View();

        }

		[HttpPost("FinanceiroPagar/ListarDadosFinanceiro")]
		public IActionResult ListarDadosFinanceiroPagar(string mesano)
		{
			var mes = int.Parse(mesano.Split('/')[0]);
			var ano = int.Parse(mesano.Split('/')[1]);

			var lstSolicitacao = _lancamentoFinanceiroTotalRepository.FinanceiroPagar(ano, mes);
			return ResponseResult(true, content: lstSolicitacao);
		}

		[HttpGet("FinanceiroPagar/PagarLancamentos")]
		public ActionResult PagarLancamentos(int idVistoriador, string mesano)
		{
			var mes = int.Parse(mesano.Split('/')[0]);
			var ano = int.Parse(mesano.Split('/')[1]);

			var lstSolicitacao = _lancamentoFinanceiroTotalRepository.FinanceiroLancamentosPagar(idVistoriador, ano, mes);
			ViewData["NomeVistoriador"] = _operadorService.Buscar(idVistoriador)?.NomeOperador;
			ViewData["Mes"] = mes;
			ViewData["Ano"] = ano;
			return View("PagarLancamentos", lstSolicitacao);
		}


		[HttpPost("FinanceiroPagar/Pagar")]
		[ServiceFilter(typeof(TransactionFilter))]
		public async Task<JsonResult> Pagar(int[] Id, string mesano)
		{
			throw new NotImplementedException();
			var mes = int.Parse(mesano.Split('/')[0]);
			var ano = int.Parse(mesano.Split('/')[1]);

			//await _lancamentoFinanceiroTotalService.Pagar(Id, ano, mes);
			await AppSaveChangesAsync();
			return ResponseResult(true);
		}

		#endregion

		#region Aux

		private static List<KeyValuePair<string, string>> GerarCompetencias(DateTime dataInicio, DateTime dataFim)
        {
            var listaCompetencias = new List<KeyValuePair<string, string>>();

            var dataAtual = dataFim;

            while (dataAtual >= dataInicio)
            {
                var key = $"{dataAtual.Month:D2}/{dataAtual.Year}";
                var value = $"{dataAtual.ToString("MMMM", new System.Globalization.CultureInfo("pt-BR"))}/{dataAtual.Year}";

                listaCompetencias.Add(new KeyValuePair<string, string>(key, value));

                dataAtual = dataAtual.AddMonths(-1);
            }

            return listaCompetencias;
        }

        #endregion
	}
}