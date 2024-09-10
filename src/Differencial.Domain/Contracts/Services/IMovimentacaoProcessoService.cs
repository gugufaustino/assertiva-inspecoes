using Differencial.Domain.Contracts.Entities;
using Differencial.Domain.Entities;
using Differencial.Domain.Filters;
using System.Collections.Generic;

namespace Differencial.Domain.Contracts.Services
{
	public interface IMovimentacaoProcessoService
	{
		IEnumerable<MovimentacaoProcesso> Listar(MovimentacaoProcessoFilter filtro);

		void Inserir(IWorkFlowMovimentacaoProcesso entidade);

        void Editar(MovimentacaoProcesso entidade);

        void Excluir(int[] id);

        void Excluir(int id);
    }
}