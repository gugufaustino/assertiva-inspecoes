using Differencial.Domain.Entities;
using Differencial.Domain.Filters;
using System.Collections.Generic;

namespace Differencial.Domain.Contracts.Services
{
    public interface IAnalistaService
	{
		IEnumerable<Analista> Listar(AnalistaFilter filtro);

		void Salvar(Analista entidade);

		void Excluir(int id);
	}
}