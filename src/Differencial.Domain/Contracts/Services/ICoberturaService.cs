using Differencial.Domain.Entities;
using Differencial.Domain.Filters;
using System.Collections.Generic;

namespace Differencial.Domain.Contracts.Services
{
    public interface ICoberturaService
    {
        IEnumerable<Cobertura> Listar(CoberturaFilter filtro);
        void Salvar(IEnumerable<Cobertura> entidade, int IdSolicitacao);
       
        void Excluir(int[] ids);
		void Excluir(ICollection<Cobertura> cobertura);
	}
}