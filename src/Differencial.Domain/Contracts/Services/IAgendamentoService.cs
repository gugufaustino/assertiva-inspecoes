using Differencial.Domain.Entities;
using Differencial.Domain.Filters;
using System.Collections.Generic;
using System;
namespace Differencial.Domain.Contracts.Services
{
	public interface IAgendamentoService
	{
		IEnumerable<Agendamento> Listar(AgendamentoFilter filtro);
		void Salvar(Agendamento entidade);
		void Excluir(int codigoUsuarioLogado, int id);
        void Agendar(DateTime dateTime, Solicitacao solicitacao);
        void Reagendar(DateTime dateTime, string motivoCancelamentoReagendamento, Solicitacao solicitacao);
        void Comunicar( Solicitacao solicitacao, string mensagem);
        IEnumerable<Agendamento> ListarAgendamentosVistoriadorDiaVigentes(int idVistoriador, DateTime dataAgenda);
        IEnumerable<Agendamento> ListarAgendamentosVistoriadorDiaVigentesCancelada(int idVistoriador, DateTime dataAgenda, int IdSolicCancelada);
		void Excluir(ICollection<Agendamento> agendamento);
	}
}