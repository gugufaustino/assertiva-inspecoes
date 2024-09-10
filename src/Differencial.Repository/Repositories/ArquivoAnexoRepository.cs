using Differencial.Domain.Contracts.Infra;
using Differencial.Domain.Contracts.Repositories;
using Differencial.Domain.Entities;
using Differencial.Domain.Filters;
using Differencial.Repository.Context;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System;
using Differencial.Domain;
using Microsoft.EntityFrameworkCore;


namespace Differencial.Repository.Repositories
{
    public class ArquivoAnexoRepository : RepositoryBase<ArquivoAnexo>, IArquivoAnexoRepository
	{
		public ArquivoAnexoRepository(IDbContextFactory dbContextFactory, IUsuarioService usuario)
            : base(dbContextFactory, usuario)
        {
		}

        public IEnumerable<ArquivoAnexo> ListarArquivoSolicitacao(int idSolicitacao, TipoArquivoAnexoEnum tipoArquivoAnexoEnum)
        {
            return _dbSet.Include(i=> i.LaudoFoto)
                            .Where(i => i.IdSolicitacao == idSolicitacao && i.TipoArquivoAnexo == tipoArquivoAnexoEnum).ToList();
        }

        public override IEnumerable<ArquivoAnexo> Where<F>(F filter)
		{
			var query = from fotos in _db.Foto
						select fotos;

			this.AplicarFiltro(ref query, filter as ArquivoAnexoFilter);

			return query.ToList();
		}

		private void AplicarFiltro(ref IQueryable<ArquivoAnexo> query, ArquivoAnexoFilter filter)
		{
			// Ordenação
			string order = string.Format("{0} {1}", filter.CampoOrdenacao.ToString(), filter.Order.ToString());
			query = query.OrderBy(order);

			if (filter.Id.HasValue)
				query = query.Where(x => filter.Id == x.Id);

            if (filter.IdSolicitacao.HasValue)
                query = query.Where(x => filter.IdSolicitacao == x.IdSolicitacao);

            if(filter.TipoArquivoAnexoEnum != null)
                query = query.Where(x => filter.TipoArquivoAnexoEnum == x.TipoArquivoAnexo);

            // Filtro
            base.ApplyBasicFilter(ref query, ref filter);
		}
	}
}