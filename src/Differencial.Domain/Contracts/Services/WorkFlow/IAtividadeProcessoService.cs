using Differencial.Domain.Contracts.Entities;
using Differencial.Domain.Entities;
using Differencial.Domain.Filters;
using System.Collections.Generic;

namespace Differencial.Domain.Contracts.Services
{
	public interface IAtividadeProcessoService
	{
		IEnumerable<AtividadeProcesso> Listar(AtividadeProcessoFilter filtro); 
        List<AtividadeProcesso> Criar(IWorkFlowInstanciaProcesso processo); 
        void Concluir(TipoAtividadeEnum tipoAtividade, IWorkFlowInstanciaProcesso processo);
        AtividadeProcesso Buscar(TipoAtividadeEnum tipoAtividade, IWorkFlowInstanciaProcesso processo);
        
        void Excluir(int[] ids);
		void Excluir(ICollection<AtividadeProcesso> atividadeProcesso);
	}
}