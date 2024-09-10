using Differencial.Domain.Contracts.Repositories;
using Differencial.Domain.Contracts.Services;
using Differencial.Domain.Entities;
using Differencial.Domain.Filters;
using Differencial.Domain.UOW;
using System.Collections.Generic;

namespace Differencial.Service.Services
{
	public class NotificacaoOperadorService : Service, INotificacaoOperadorService
	{
		INotificacaoOperadorRepository _notificacaoOperadorRepositorio;

		public NotificacaoOperadorService(IUnitOfWork uow, INotificacaoOperadorRepository notificacaoOperadorRepositorio)
			: base(uow)
		{
			_notificacaoOperadorRepositorio = notificacaoOperadorRepositorio;
		}

		public IEnumerable<NotificacaoOperador> MinhasNotificacoes(int idOperador)
		{
			return TryCatch(() =>
			{
				return _notificacaoOperadorRepositorio.MinhasNotificacoes(idOperador);
			});
		}

		public void Salvar( NotificacaoOperador entidade)
		{
			TryCatch(() =>
			{
				entidade.Validate();

				if (entidade.Id == 0)
					_notificacaoOperadorRepositorio.Add( entidade);
				else
					_notificacaoOperadorRepositorio.Update( entidade);
			});
		}

        public void Salvar(List<NotificacaoOperador> entidades)
        {
            entidades.ForEach((entidade) =>
            {
                Salvar(entidade);

            });
        }

        public void Excluir( int id)
		{
			TryCatch(() =>
			{
				_notificacaoOperadorRepositorio.Delete( id);
			});
		}
	}
}