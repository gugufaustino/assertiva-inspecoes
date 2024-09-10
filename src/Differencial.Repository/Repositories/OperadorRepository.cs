using Differencial.Domain.Contracts.Infra;
using Differencial.Domain.Contracts.Repositories;
using Differencial.Domain.Entities;
using Differencial.Domain.Filters;
using Differencial.Domain.Util.ExtensionMethods;
using Differencial.Repository.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Differencial.Repository.Repositories
{
    public class OperadorRepository : RepositoryBase<Operador>, IOperadorRepository
    {
        public OperadorRepository(IDbContextFactory dbContextFactory, IUsuarioService usuario)
            : base(dbContextFactory, usuario)
        {
        }

        public async Task<Operador> BuscarParaEditarView(int id)
        {
            return await _dbSet
                    .Include(i => i.Endereco)
                    .Include(i => i.Vistoriador).ThenInclude(i=> i.EnderecoBase)
                    .FirstOrDefaultAsync(i => i.Id == id);
        }
        public async Task<Operador> BuscarParaEditarUpdate(int id)
        {
            return await _dbSet
                    .Include(i => i.Endereco)
                    .Include(i => i.Vistoriador).ThenInclude(i=> i.EnderecoBase)
                    .Include(i => i.Solicitante)
                    .Include(i => i.Analista)
                    .FirstOrDefaultAsync(i => i.Id == id);
        }

        public IEnumerable<Operador> Listar(OperadorFilter filter)
        {
            var query = from operador
                        in _db.Operador
                        .Where(o => o.Solicitante == null || o.Solicitante.TipoSolicitante == Domain.TipoSolicitanteEnum.AcessoAoSistema)
                        select operador;

            this.AplicarFiltro(ref query, filter);
            var lst = query.ToList();
            return lst;

        }

        public override IEnumerable<Operador> Where<F>(F filter)
        {
            var query = from operador in _db.Operador
                        select operador;

            this.AplicarFiltro(ref query, filter as OperadorFilter);
            var lst = query.ToList();
            return lst;
        }

        private void AplicarFiltro(ref IQueryable<Operador> query, OperadorFilter filter)
        {
            // Ordenação
            string order = string.Format("{0} {1}", filter.CampoOrdenacao.ToString(), filter.Order.ToString());
            query = query.OrderBy(order);

            if (filter.Id.HasValue)
                query = query.Where(x => filter.Id == x.Id);

            if (filter.NomeOperador.IsNullOrEmpty() == false)
                query = query.Where(x => x.NomeOperador.Contains(filter.NomeOperador));

            if (filter.Email.IsNullOrEmpty() == false)
                query = query.Where(x => x.Email.Contains(filter.Email));

            if (filter.Cpf.IsNullOrEmpty() == false)
                query = query.Where(x => x.Cpf.Contains(filter.Cpf));

            if (filter.Rg.IsNullOrEmpty() == false)
                query = query.Where(x => x.Rg.Contains(filter.Rg));

            if (filter.DataNascimento.HasValue)
                query = query.Where(x => filter.DataNascimento == x.DataNascimento);

            if (filter.IndAnalista.HasValue)
                query = query.Where(x => filter.IndAnalista == x.IndAnalista);

            if (filter.IndGerente.HasValue)
                query = query.Where(x => filter.IndGerente == x.IndGerente);

            if (filter.IndVistoriador.HasValue)
                query = query.Where(x => filter.IndVistoriador == x.IndVistoriador);

            if (filter.IndSolicitante.HasValue)
                query = query.Where(x => filter.IndSolicitante == x.IndSolicitante);

            if (filter.DataCadastro.HasValue)
                query = query.Where(x => filter.DataCadastro == x.DataCadastro);

            if (filter.DataModificacao.HasValue)
                query = query.Where(x => filter.DataModificacao == x.DataModificacao);

            if (filter.IndAtivo.HasValue)
                query = query.Where(x => filter.IndAtivo == x.IndAtivo);

            // Filtro
            base.ApplyBasicFilter(ref query, ref filter);
        }


    }
}