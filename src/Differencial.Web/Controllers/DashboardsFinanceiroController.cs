using Differencial.Domain.Contracts.Infra;
using Differencial.Domain.Contracts.Repositories;
using Differencial.Domain.Contracts.Services;
using Differencial.Domain.UOW;
using Differencial.Domain.Util.ExtensionMethods;
using Differencial.Service.Services;
using Differencial.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WEB.Controllers
{

	[Route("Dashboards/FinanceiroReceber")]
	public class DashboardsFinanceiroController : BaseController
	{ 
		private readonly ISeguradoraService _seguradoraService; 
		private readonly ILancamentoFinanceiroTotalRepository _lancamentoFinanceiroTotalRepository;
		private readonly ILancamentoFinanceiroTotalService _lancamentoFinanceiroTotalService; 

		public DashboardsFinanceiroController( 
			ISeguradoraService seguradoraService, 
			ILancamentoFinanceiroTotalRepository lancamentoFinanceiroTotalRepository, 
			ILancamentoFinanceiroTotalService lancamentoFinanceiroTotalService)
		{
			 
			_seguradoraService = seguradoraService; 
			_lancamentoFinanceiroTotalRepository = lancamentoFinanceiroTotalRepository;
			_lancamentoFinanceiroTotalService = lancamentoFinanceiroTotalService;


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


		[HttpPost("Faturar")]
		[ServiceFilter(typeof(TransactionFilter))]
		public async Task<JsonResult> Faturar(int[] Id, string mesano)
		{
			var mes = int.Parse(mesano.Split('/')[0]);
			var ano = int.Parse(mesano.Split('/')[1]);

			await _lancamentoFinanceiroTotalService.Faturar(Id, ano, mes);
			await AppSaveChangesAsync();
			return ResponseResult(true);
		}

		[HttpPost("Liquidar")]
		[ServiceFilter(typeof(TransactionFilter))]
		public async Task<JsonResult> Liquidar(int[] Id, string mesano)
		{
			var mes = int.Parse(mesano.Split('/')[0]);
			var ano = int.Parse(mesano.Split('/')[1]);

			await _lancamentoFinanceiroTotalService.Liquidar(Id, ano, mes);
			await AppSaveChangesAsync();
			return ResponseResult(true);
		}
	}
}