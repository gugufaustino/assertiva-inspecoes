using Dapper;
using Differencial.Domain.Contracts.Infra;
using Differencial.Domain.Contracts.Util;
using Differencial.Domain.Filters;
using Differencial.Domain.Queries;
using Differencial.Domain.Queries.Dao;
using System.Collections.Generic;
using System.Linq;
using System;
using Differencial.Domain;

namespace Differencial.Queries.Queries
{
    public class SolicitacaoQueries : QueriesBase<SolicitacaoRootDao, SolicitacaoFilter>, ISolicitacaoQueries
    {
        public SolicitacaoQueries(IUsuarioService usuario, IConfiguracaoAplicativo configuracao)
            : base(usuario, configuracao)
        {
        }
        public List<SolicitacaoCobrancaVistoriaDao> SolicitacoesCobrancaVistoria()
        {
            /*
             	--DATEDIFF(HOUR, DthMovimentacao , GETDATE()) as MovimentacaoHorasDecorridas,
	            --CASE WHEN DATEDIFF(DAY, ControleDthEmailCobrancaVistoria , GETDATE()) = 0 THEN 1 ELSE 0 END  as IndExecutadoHoje 
             */
            var sql = $@"select ControleDthEmailCobrancaVistoria,
                        Solicitacao.Id, CodSeguradora, NomeRazaoSocial,
                        Endereco.Logradouro, Endereco.Numero, Endereco.Complemento,Endereco.Bairro, Endereco.Cep, Endereco.NomeMunicipio, Endereco.SiglaUf,
                        Operador.NomeOperador, Email as EmailOperador, DthMovimentacao
                        from Solicitacao
                        inner join MovimentacaoProcesso on Solicitacao.Id = MovimentacaoProcesso.IdSolicitacao
                        inner join Vistoriador on Solicitacao.IdVistoriador = Vistoriador.id
                        inner join Operador on Operador.Id = Vistoriador.Id
                        inner join Cliente on Cliente.Id = Solicitacao.IdCliente
                        inner join Endereco on Endereco.Id = Solicitacao.IdEnderecoCliente
                        where IndRelacionamentoAgendaInformada = 0 and MovimentacaoProcesso.TipoSituacaoProcesso in ({(int)TipoSituacaoProcessoEnum.EnviadoParaVistoria}, {(int)TipoSituacaoProcessoEnum.ApropriadoVistoriador})
                        and DATEDIFF(HOUR, DthMovimentacao , GETDATE()) >= 24 "; //--MovimentacaoHorasDecorridas

            return _conn.Query<SolicitacaoCobrancaVistoriaDao>(sql).ToList();


        }
    }
}