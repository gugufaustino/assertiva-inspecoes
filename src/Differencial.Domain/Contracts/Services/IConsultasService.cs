using Differencial.Domain.Entities;
using Differencial.Domain.Filters;
using System.Collections.Generic;
namespace Differencial.Domain.Contracts.Services
{
    public interface IConsultasService //: IBaseService<Solicitacao, SolicitacaoFilter>
    {
        IEnumerable<Solicitacao> ListarEmTramitacao();
        IEnumerable<Solicitacao> ListarConcluidas();
        IEnumerable<Solicitacao> ListarTodasAgendas();
        IEnumerable<Solicitacao> ListarTodasSolicitacoes();
		IEnumerable<Solicitacao> ListarTodasRotas(SolicitacaoFilter solicitacaoFilter);
	}
}