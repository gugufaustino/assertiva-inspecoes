using Differencial.Domain.Entities;
using Differencial.Domain.Filters;
using System.Collections.Generic;

namespace Differencial.Domain.Contracts.Services
{
	public interface IClienteService
	{
		IEnumerable<Cliente> Listar(ClienteFilter filtro);

		void Salvar(Cliente entidade);

		void Excluir(int id);
	}
}