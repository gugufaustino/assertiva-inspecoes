using Differencial.Domain.Entities;
using Differencial.Domain.Filters;
using System.Collections.Generic;

namespace Differencial.Domain.Contracts.Services
{
	public interface IAnalistaProdutoService
	{
		IEnumerable<AnalistaProduto> Listar(AnalistaProdutoFilter filtro);

		void Salvar(int codigoUsuarioLogado, AnalistaProduto entidade);

		void Excluir(int codigoUsuarioLogado, int id);
	}
}