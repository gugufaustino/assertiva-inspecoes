using Differencial.Domain.Entities;
using Differencial.Domain.Filters;
using System.Collections.Generic;

namespace Differencial.Domain.Contracts.Services
{
	public interface INotificacaoOperadorService
	{

		void Salvar( NotificacaoOperador entidade);

        void Salvar(List<NotificacaoOperador> entidades);

        void Excluir( int id);
        IEnumerable<NotificacaoOperador> MinhasNotificacoes(int idOperador);
    }
}