using Differencial.Domain.Contracts.Infra;
using Differencial.Domain.Contracts.Repositories;
using Differencial.Domain.Entities;
using Differencial.Domain.Filters;
using Differencial.Repository.Context;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using Differencial.Repository.Repositories.Base;

namespace Differencial.Repository.Repositories
{
    public class AgendamentoRepository : RepositoryBase<Agendamento>, IAgendamentoRepository
	{
		public AgendamentoRepository(IDbContextFactory dbContextFactory, IUsuarioService usuario)
			: base(dbContextFactory, usuario)
		{
		}

        public IEnumerable<Agendamento> ListarAgendamentosVistoriadorDiaVigentes(int idVistoriador, DateTime dataAgenda)
        {
            var lst = _db.Agendamento
								.Include(i=> i.Solicitacao.Vistoriador.EnderecoBase)
								.Include(i=> i.Solicitacao.Vistoriador.Operador)
								.Include(i=> i.Solicitacao.Vistoriador).ThenInclude(vp=> vp.VistoriadorProduto)
								.Include(i=> i.Solicitacao.Endereco)
								.Where(a => a.IndCancelado == false
									&& a.TipoAgendamento != Domain.TipoAgendamentoEnum.Comunicar
									&& a.IdVistoriador == idVistoriador
									&& (a.DthAgendamento ?? default).Date == dataAgenda.Date);

            return lst.OrderBy(o=>o.DthAgendamento);
        }

        public IEnumerable<Agendamento> ListarAgendamentosVistoriadorDiaVigentesCancelada(int idVistoriador, DateTime dataAgenda, int IdSolicCancelada)
        {
            var lst = _db.Agendamento.Where(a => a.IndCancelado == false
                                            && a.TipoAgendamento != Domain.TipoAgendamentoEnum.Comunicar
                                            && a.IdVistoriador == idVistoriador
                                            && (a.DthAgendamento ?? default).Date == dataAgenda.Date
                                            || (a.IdVistoriador == idVistoriador
                                                && IdSolicCancelada == a.Solicitacao.Id 
                                                && a.DthAgendamento == dataAgenda));// Foi necessário aplicar esse tratamento para remover outra agenda cancelada 


            return lst.OrderBy(o => o.DthAgendamento);
        }
        public override IEnumerable<Agendamento> Where<F>(F filter)
		{
			var query = from agendamento in _db.Agendamento
						select agendamento;

			this.AplicarFiltro(ref query, filter as AgendamentoFilter);

			return query.ToList();
		}


        private void AplicarFiltro<F>(ref IQueryable<Agendamento> query, F filter) where F : AgendamentoFilter, new()
		{
			// Ordenação
			string order = string.Format("{0} {1}", filter.CampoOrdenacao.ToString(), filter.Order.ToString());
			query = query.OrderBy(order);

			if (filter.Id.HasValue)
				query = query.Where(x => filter.Id == x.Id);

			// Filtro
			base.ApplyBasicFilter(ref query, ref filter);
		}
	}
}