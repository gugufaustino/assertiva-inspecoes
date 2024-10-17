using Differencial.Domain.Entities;
using Differencial.Domain.Filters;
using System.Collections.Generic;
namespace Differencial.Domain.Contracts.Services
{
    public interface IConsultasService  
    {
        IEnumerable<Solicitacao> ListarEmTramitacao();
        IEnumerable<Solicitacao> ListarConcluidas();
        IEnumerable<Solicitacao> ListarTodasAgendas();
        IEnumerable<Solicitacao> ListarTodasSolicitacoes();
	 
	}
}