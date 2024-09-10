using Differencial.Domain.Entities;
using Differencial.Domain.Filters;
using System.Collections.Generic;

namespace Differencial.Domain.Contracts.Services
{
	public interface ISolicitanteService
	{
		IEnumerable<Solicitante> Listar(SolicitanteFilter filtro);

		void Salvar( Solicitante entidade);

		void Excluir( int id);
		IEnumerable<Solicitante> ddlSolicitante(int idSeguradora);
    }
}