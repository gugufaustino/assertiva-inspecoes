using Differencial.Domain.DTO;
using Differencial.Domain.Entities;
using Differencial.Domain.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Differencial.Domain.Contracts.Services
{
    public interface IProdutoService : IBaseService<Produto, ProdutoFilter>
    {
        IEnumerable<VistoriadorProdutoValorDTO> ListarDiponivelParaVistoriador(int idVistoriador);
        Produto BuscarPorCodProdutoSeguradora(string codProdutoSeguradora);
        bool ExisteDadosFinanceiros(int idProduto);
        Task<Produto> BuscarParaEditar(int id);
    }
}