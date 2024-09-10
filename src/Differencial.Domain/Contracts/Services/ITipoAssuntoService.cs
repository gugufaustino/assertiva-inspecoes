using Differencial.Domain.Entities;
using Differencial.Domain.Filters;
using System.Collections.Generic;

namespace Differencial.Domain.Contracts.Services
{
	public interface ITipoAssuntoService
	{
		IEnumerable<TipoAssunto> Listar(TipoAssuntoFilter filtro);

		void Salvar( TipoAssunto entidade);

		void Excluir(int id);

        void Excluir(int[] ids);

        TipoAssunto Buscar(int id);
    }
}