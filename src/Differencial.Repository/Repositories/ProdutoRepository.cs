using Differencial.Domain.Contracts.Repositories;
using Differencial.Domain.Entities;
using Differencial.Domain.Filters;
using Differencial.Domain.Util.ExtensionMethods;
using Differencial.Repository.Context;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using Differencial.Domain.DTO;
using Differencial.Domain.Contracts.Infra;
using Microsoft.EntityFrameworkCore;

namespace Differencial.Repository.Repositories
{
    public class ProdutoRepository : RepositoryBase<Produto>, IProdutoRepository
    {
        public ProdutoRepository(IDbContextFactory dbContextFactory, IUsuarioService usuario)
            : base(dbContextFactory, usuario)
        {
        }

        public IEnumerable<VistoriadorProdutoValorDTO> ListarDiponivelParaVistoriador(int idVistoriador)
        {
            var queryProduto = (from produto in _db.Produto
                                join vistoriadorproduto in _db.VistoriadorProduto.Where(i => i.IdVistoriador == idVistoriador)
                                        on new { IdProduto = produto.Id, IdContratoLancamentoValor = 0 }
                                        equals new { vistoriadorproduto.IdProduto, vistoriadorproduto.IdContratoLancamentoValor } into vistoriadorProdutoTmp
                                from vistoriadorProdutoDefault in vistoriadorProdutoTmp.DefaultIfEmpty()
                                select new VistoriadorProdutoValorDTO()
                                {
                                    IdProduto = produto.Id,
                                    IdVistoriadorProduto = vistoriadorProdutoDefault == null ? 0 : vistoriadorProdutoDefault.Id,
                                    IdVistoriador = vistoriadorProdutoDefault == null ? 0 : vistoriadorProdutoDefault.IdVistoriador,
                                    IdContratoLancamento = 0,
                                    IdContratoLancamentoValor = 0,
                                    TipoParametroQuantitativoVariavel = Domain.TipoContratoParametroEnum.Comum,
                                    NomeProduto = produto.NomeProduto,
                                    NomeSeguradora = produto.Seguradora.NomeSeguradora,
                                    VistoriadorProduto = vistoriadorProdutoDefault,

                                }).ToList().Distinct();

            var queryLancamentoValor = (from contrato in _db.Contrato
                                        join produto in _db.Produto on contrato.Id equals produto.Id
                                        join contratolancamento in _db.ContratoLancamento on contrato.Id equals contratolancamento.IdContrato
                                        join contratoLancamentoValor in _db.ContratoLancamentoValor on contratolancamento.Id equals contratoLancamentoValor.IdContratoLancamento
                                        join vistoriadorproduto in _db.VistoriadorProduto.Where(i => i.IdVistoriador == idVistoriador)
                                            on new { IdProduto = produto.Id, IdContratoLancamentoValor = contratoLancamentoValor.Id }
                                            equals new { vistoriadorproduto.IdProduto, vistoriadorproduto.IdContratoLancamentoValor } into vistoriadorProdutoTmp
                                        from vistoriadorProdutoDefault in vistoriadorProdutoTmp.DefaultIfEmpty()

                                        where contratolancamento.TipoParametroQuantitativoVariavel == Domain.TipoContratoParametroEnum.ValorRisco
                                        select new VistoriadorProdutoValorDTO
                                        {
                                            IdProduto = produto.Id,
                                            IdVistoriadorProduto = vistoriadorProdutoDefault == null ? 0 : vistoriadorProdutoDefault.Id,
                                            IdVistoriador = vistoriadorProdutoDefault == null ? 0 : vistoriadorProdutoDefault.IdVistoriador,
                                            IdContratoLancamento = vistoriadorProdutoDefault == null ? contratoLancamentoValor.IdContratoLancamento : vistoriadorProdutoDefault.IdContratoLancamento,
                                            IdContratoLancamentoValor = vistoriadorProdutoDefault == null ? contratoLancamentoValor.Id : vistoriadorProdutoDefault.IdContratoLancamentoValor,
                                            TipoParametroQuantitativoVariavel = contratolancamento.TipoParametroQuantitativoVariavel,
                                            NomeProduto = produto.NomeProduto,
                                            NomeSeguradora = produto.Seguradora.NomeSeguradora,
                                            VistoriadorProduto = vistoriadorProdutoDefault,
                                            ContratoLancamentoValor = contratoLancamentoValor
                                        }).ToList();
            var lst = queryProduto.Union(queryLancamentoValor).ToList();

            lst.ForEach(delegate (VistoriadorProdutoValorDTO i)
            {
                i.KeyVistoriadorProdutoLancamentoValor = new KeyVistoriadorProdutoLancamentoDTO
                {
                    IdVistoriadorProduto = i.IdVistoriadorProduto,
                    IdProduto = i.IdProduto,
                    IdContratoLancamento = i.ContratoLancamentoValor == null ? 0 : i.ContratoLancamentoValor.IdContratoLancamento,
                    IdContratoLancamentoValor = i.ContratoLancamentoValor == null ? 0 : i.ContratoLancamentoValor.Id
                };
            });

            return lst;
        }

        public IEnumerable<Produto> Listar(ProdutoFilter filter)
        {
            var query = _dbSet
                        .Include(i => i.Seguradora)
                        .Include(i => i.TipoInspecao)
                        .AsNoTracking();

            this.AplicarFiltro(ref query, filter);

            return query;
        }

        public override IEnumerable<Produto> Where<F>(F filter)
        {
            var query = from produto in _db.Produto select produto;
            this.AplicarFiltro(ref query, filter as ProdutoFilter);
            return query.ToList();
        }

        private void AplicarFiltro(ref IQueryable<Produto> query, ProdutoFilter filter)
        {
            // Ordenação
            string order = string.Format("{0} {1}", filter.CampoOrdenacao.ToString(), filter.Order.ToString());
            query = query.OrderBy(order);

            if (filter.Id.HasValue)
                query = query.Where(x => filter.Id == x.Id);

            if (filter.IdSeguradora.HasValue)
                query = query.Where(x => filter.IdSeguradora == x.IdSeguradora);

            if (filter.IdTipoInspecao.HasValue)
                query = query.Where(x => filter.IdTipoInspecao == x.IdTipoInspecao);

            if (filter.NomeProduto.IsNullOrEmpty() == false)
                query = query.Where(x => x.NomeProduto.Contains(filter.NomeProduto));

            // Filtro
            base.ApplyBasicFilter(ref query, ref filter);
        }

        public bool ExisteDadosFinanceiros(int idProduto)
        {
            return _db.ContratoLancamento.Any(i => i.Contrato.Produto.Id == idProduto);
        }
    }
}