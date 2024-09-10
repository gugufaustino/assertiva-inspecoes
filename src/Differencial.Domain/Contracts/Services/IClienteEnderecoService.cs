using Differencial.Domain.Entities;
using Differencial.Domain.Filters;
using System.Collections.Generic;

namespace Differencial.Domain.Contracts.Services
{
	public interface IClienteEnderecoService
	{
		IEnumerable<ClienteEndereco> Listar(ClienteEnderecoFilter filtro);

		void Salvar(int codigoUsuarioLogado, ClienteEndereco entidade);

		void Excluir(int codigoUsuarioLogado, int id);
	}
}