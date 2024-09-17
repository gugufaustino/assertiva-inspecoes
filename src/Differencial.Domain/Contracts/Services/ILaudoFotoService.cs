using Differencial.Domain.Entities;
using Differencial.Domain.Filters;
using System;
using System.Collections.Generic;

namespace Differencial.Domain.Contracts.Services
{
	public interface ILaudoFotoService
	{
		IEnumerable<LaudoFoto> Listar(LaudoFotoFilter filtro);

		void Salvar( LaudoFoto entidade);

 

        void SalvarQuadroFotoPosicao(int idSolicitacao, Dictionary<int, int> dicQuadroFotoPosicao);

        void SalvarArquivoSolicitacaoQuadroFotosRemover(int id, int idSolicitacao);
        void ExcluirFotoLaudoFoto(int idSolicitacao, Guid guid);
		void ExcluirFotoLaudoFoto(ICollection<ArquivoAnexo> foto);
	}
}