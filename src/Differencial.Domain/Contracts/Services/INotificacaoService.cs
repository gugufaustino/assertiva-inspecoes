using Differencial.Domain.Contracts.Entities;
using Differencial.Domain.Entities;
using Differencial.Domain.Filters;
using System.Collections.Generic;

namespace Differencial.Domain.Contracts.Services
{
	public interface INotificacaoService
	{
		IEnumerable<Notificacao> Listar(NotificacaoFilter filtro);

		void Salvar( Notificacao entidade);

		void Excluir(int id);
        void SalvarTransmitirNotificacaoProcesso(INotificacaoProcesso processo);
        List<NotificacaoOperador> MinhasNotificacoes();
        void ExcluirTodasMinhas();
    }
}