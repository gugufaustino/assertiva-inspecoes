using Differencial.Domain.Contracts.Infra;
using Differencial.Domain.Contracts.Repositories;
using Differencial.Domain.Entities;
using Differencial.Domain.EntitiesDTO;
using Differencial.Domain.Filters;
using Differencial.Repository.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace Differencial.Repository.Repositories
{
    public class LancamentoFinanceiroTotalRepository : RepositoryBase<LancamentoFinanceiroTotal>, ILancamentoFinanceiroTotalRepository
    {
        public LancamentoFinanceiroTotalRepository(IDbContextFactory dbContextFactory, IUsuarioService usuario)
            : base(dbContextFactory, usuario)
        {
        }

        public IEnumerable<LancamentoFinanceiroTotal> TodosLancamentosFinanceiros()
        {
            var query = _db.LancamentoFinanceiroTotal
                            .Include(i => i.Solicitacao)
                            .AsNoTracking();



            return query.ToList();
        }
                
        public IEnumerable<FinanceiroReceberDto> FinanceiroReceber( int ano, int mes)
        {
            var query = from solicitacao in _db.Solicitacao
                        join lancamento in _db.LancamentoFinanceiroTotal 
                                            on solicitacao.Id equals lancamento.IdSolicitacao
                        join seguradora in _db.Seguradora on solicitacao.IdSeguradora equals seguradora.Id
                        group new { lancamento.TipoLancamentoFinanceiro, lancamento.ValorLancamentoFinanceiroTotal, lancamento.DthLancamentoPagamento, seguradora.NomeSeguradora, solicitacao.TpSituacao, }
                            by new { solicitacao.IdSeguradora , seguradora.NomeSeguradora, lancamento.TipoLancamentoFinanceiro, 
                                    Mes = lancamento.DthLancamentoPagamento.Date.Month,
                                    Ano = lancamento.DthLancamentoPagamento.Date.Year,
                            } into gp                        
                        where gp.Key.TipoLancamentoFinanceiro == Domain.TipoLancamentoFinanceiroEnum.ReceitaProdutoReceberSeguradora

                        select new FinanceiroReceberDto(
                                        gp.Key.IdSeguradora,
                                        gp.Key.NomeSeguradora,
                                        gp.Key.TipoLancamentoFinanceiro,
                                        gp.Min(i => i.TpSituacao),
                                        gp.Sum(i => i.ValorLancamentoFinanceiroTotal), 
                                        gp.Key.Ano,
                                        gp.Key.Mes); 
           
            return query.ToList();
        }

        public IEnumerable<FinanceiroLancamentosReceberDto> FinanceiroLancamentosReceber(int id, int ano, int mes)
        {
            var query = from solicitacao in _db.Solicitacao
                        join cliente  in _db.Cliente on solicitacao.IdCliente equals cliente.Id
                        join endereco  in _db.Endereco on solicitacao.IdEnderecoCliente equals endereco.Id
                        join vistoriador  in _db.Vistoriador on solicitacao.IdVistoriador equals vistoriador.Id
                        join operadorVistoriador in _db.Operador on vistoriador.Id equals operadorVistoriador.Id
                        join vistoriadorEndereco in _db.Endereco on vistoriador.IdEnderecoBase equals vistoriadorEndereco.Id

                        join lancamento in _db.LancamentoFinanceiroTotal on solicitacao.Id equals lancamento.IdSolicitacao
                        join seguradora in _db.Seguradora on solicitacao.IdSeguradora equals seguradora.Id
                        where lancamento.TipoLancamentoFinanceiro == Domain.TipoLancamentoFinanceiroEnum.ReceitaProdutoReceberSeguradora
                                && solicitacao.IdSeguradora == id

                        select new FinanceiroLancamentosReceberDto(
                            solicitacao.Id,
                            "solicitacaoProposta",
                            solicitacao.CodSeguradora,
                            "centro de custo",
                            solicitacao.DataCadastro,
                            "solicitacao.Agendamento.",
                            "Data Envio",
                            "solicitante",
                            cliente.AtividadeNome,
                            cliente.CpfCnpj + " - " + cliente.NomeRazaoSocial,
                            endereco.Logradouro, endereco.NomeMunicipio, endereco.SiglaUf,

                            solicitacao.VistoriadorCidadeBase,
                            vistoriadorEndereco.SiglaUf,
                            operadorVistoriador.NomeOperador,

                            "Pedágio",
                            solicitacao.DeslocamentoRealizado,
                            "Km base",
                            solicitacao.VlrQuilometroRodado,
                            solicitacao.VlrPagamentoVistoria,
                            solicitacao.CustoTotalRealizado,
                            lancamento.ValorLancamentoFinanceiroTotal,
                            solicitacao.TpSituacao,
                            solicitacao.VlrRiscoSegurado,
                            "Carta de recomendação (S/N)",
                            solicitacao.TxtInformacoesAdicionais
                            );

            return query.ToList();
        }

        public override IEnumerable<LancamentoFinanceiroTotal> Where<F>(F filter)
        {
            var query = from lancamentoFinanceiroTotal in _db.LancamentoFinanceiroTotal
                        select lancamentoFinanceiroTotal;

            this.AplicarFiltro(ref query, filter as LancamentoFinanceiroTotalFilter);

            return query.ToList();
        }

        private void AplicarFiltro(ref IQueryable<LancamentoFinanceiroTotal> query, LancamentoFinanceiroTotalFilter filter)
        {
            // Ordenação
            string order = string.Format("{0} {1}", filter.CampoOrdenacao.ToString(), filter.Order.ToString());
            query = query.OrderBy(order);

            // Filtro
            base.ApplyBasicFilter(ref query, ref filter);
        }
    }


}