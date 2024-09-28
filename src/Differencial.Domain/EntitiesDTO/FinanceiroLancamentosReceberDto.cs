using System;

namespace Differencial.Domain.EntitiesDTO
{
    public record FinanceiroLancamentosReceberDto(int Id, string solicitacaoProposta,
        string CodSeguradora, string centroCusto, DateTime DataCadastro,

        string DataAgendamento, string DataEnvio, string Solicitante,

        string AtividadeNome,
        string Cliente,
        string Logradouro,
        string NomeMunicipio,
        string SiglaUf,

        string VistoriadorCidadeBase,
        string VistoriadorSiglaUf,
        string VistoriadorNome,

        string Pedagio,
        decimal? DeslocamentoRealizado,
        string KmBase,
        decimal? VlrQuilometroRodado,
        decimal? VlrPagamentoVistoria,
        decimal? CustoTotalRealizado, 
        decimal ValorLancamentoFinanceiroTotal,

        TipoSituacaoProcessoEnum? TipoSituacaoProcesso,
        decimal? VlrRiscoSegurado,
        string IndCartaRecomendacao,
        string TxtInformacoesAdicionais

        );
}
