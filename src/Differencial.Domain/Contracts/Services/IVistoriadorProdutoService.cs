using Differencial.Domain.DTO;
using Differencial.Domain.Entities;
using Differencial.Domain.Filters;
using System.Collections.Generic;

namespace Differencial.Domain.Contracts.Services
{
	public interface IVistoriadorProdutoService : IBaseAtivavelService
	{
		IEnumerable<VistoriadorProduto> Listar(VistoriadorProdutoFilter filtro);

		void Salvar(int codigoUsuarioLogado, VistoriadorProduto entidade);

		void Excluir(int codigoUsuarioLogado, int id);
        void SalvarValoresVistoriadorProduto(int idVistoriador, KeyVistoriadorProdutoLancamentoDTO[] arrVistoriadorProduto, decimal vlrQuilometroRodado, decimal vlrPagamentoVistoria);
    }
}