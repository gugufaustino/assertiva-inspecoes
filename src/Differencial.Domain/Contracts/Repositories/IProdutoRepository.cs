using System.Collections.Generic;
using Differencial.Domain.Entities;
using Differencial.Domain.Filters;
using Differencial.Domain.DTO;
using System.Threading.Tasks;

namespace Differencial.Domain.Contracts.Repositories
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        IEnumerable<VistoriadorProdutoValorDTO> ListarDiponivelParaVistoriador(int idVistoriador);

        IEnumerable<Produto> Listar(ProdutoFilter filter);
        bool ExisteDadosFinanceiros(int idProduto);
        Task<Produto> BuscarParaEditarView(int id);
    }
}