using Differencial.Domain.Entities;
using Differencial.Domain.Filters;

namespace Differencial.Domain.Contracts.Repositories
{
    public interface IContratoRepository : IRepository<Contrato>
    {
        Contrato BuscarPorProduto(int idProduto);
    }
}