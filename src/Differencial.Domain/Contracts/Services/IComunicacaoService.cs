using Differencial.Domain.Entities;
using Differencial.Domain.Filters;
using System.Collections.Generic;

namespace Differencial.Domain.Contracts.Services
{
	public interface IComunicacaoService
	{
		IEnumerable<Comunicacao> Listar(ComunicacaoFilter filtro);

		void Salvar(  Comunicacao entidade);

		void Excluir(  int id);
	}
}