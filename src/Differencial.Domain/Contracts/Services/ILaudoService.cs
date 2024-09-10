using Differencial.Domain.Entities;
using Differencial.Domain.Filters;
using System.Collections.Generic;

namespace Differencial.Domain.Contracts.Services
{
	public interface ILaudoService
	{
		IEnumerable<Laudo> Listar(LaudoFilter filtro);

		void Salvar(int codigoUsuarioLogado, Laudo entidade);

		void Excluir(int codigoUsuarioLogado, int id);

       
    }
}