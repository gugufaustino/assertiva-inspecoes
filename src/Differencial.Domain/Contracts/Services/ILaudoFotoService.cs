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

		void Excluir(  int id);

        void SalvarQuadroFotoPosicao(int idSolicitacao, Dictionary<int, int> dicQuadroFotoPosicao);

        void SalvarArquivoSolicitacaoQuadroFotosRemover(int id, int idSolicitacao);
        void ExcluirFoto(int idSolicitacao, Guid guid);
    }
}