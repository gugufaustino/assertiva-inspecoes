namespace Differencial.Domain.EntitiesDTO
{
	public record FinanceiroReceberDto(
			int Id,
			string NomeSeguradora,
			TipoLancamentoFinanceiroEnum tipoLancamentoFinanceiro,
			TipoSituacaoProcessoEnum? TpSituacao,
			decimal ValorLancamentoFinanceiro,
			int Ano,
			int Mes,
			bool IndFaturado,
			bool IndLiquidado);

	public record FinanceiroPagarDto(
			int Id,
			string NomeVistoriador,
			TipoLancamentoFinanceiroEnum tipoLancamentoFinanceiro,
			decimal ValorLancamentoFinanceiro,
			int Ano,
			int Mes,
			bool IndPago);
}
