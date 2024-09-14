using Differencial.Domain.Contracts.Infra;
using Differencial.Domain.Contracts.Repositories;
using Differencial.Domain.Entities;
using Differencial.Domain.Filters;
using Differencial.Domain.Util.ExtensionMethods;
using Differencial.Repository.Context;
using Differencial.Repository.Repositories.Base;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace Differencial.Repository.Repositories
{
    public class ComunicacaoRepository : RepositoryBase<Comunicacao>, IComunicacaoRepository
	{
		public ComunicacaoRepository(IDbContextFactory dbContextFactory, IUsuarioService usuario)
            : base(dbContextFactory, usuario)
        {
		}

		public override IEnumerable<Comunicacao> Where<F>(F filter)
		{
			var query = from comunicacao in _db.Comunicacao
						select comunicacao;

			this.AplicarFiltro(ref query, filter as ComunicacaoFilter);

			return query.ToList();
		}

		private void AplicarFiltro(ref IQueryable<Comunicacao> query, ComunicacaoFilter filter)
		{
			// Ordenação
			string order = string.Format("{0} {1}", filter.CampoOrdenacao.ToString(), filter.Order.ToString());
			query = query.OrderBy(order);

			if (filter.Id.HasValue)
				query = query.Where(x => filter.Id == x.Id);

			if (filter.IdTipoAssunto.HasValue)
				query = query.Where(x => filter.IdTipoAssunto == x.IdTipoAssunto);

			if (filter.TipoComunicacao.HasValue)
				query = query.Where(x => filter.TipoComunicacao == x.TipoComunicacao);

			if (filter.Assunto.IsNullOrEmpty() == false)
				query = query.Where(x => x.Assunto.Contains(filter.Assunto));

			if (filter.TextoComunicacao.IsNullOrEmpty() == false)
				query = query.Where(x => x.TextoComunicacao.Contains(filter.TextoComunicacao));

			if (filter.IdSolicitacao.HasValue)
				query = query.Where(x => filter.IdSolicitacao == x.IdSolicitacao);

			// Filtro
			base.ApplyBasicFilter(ref query, ref filter);
		}
	}
}