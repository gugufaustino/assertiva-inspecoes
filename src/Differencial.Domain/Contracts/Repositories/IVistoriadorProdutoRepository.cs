using Differencial.Domain.Entities;
using Differencial.Domain.Filters;

namespace Differencial.Domain.Contracts.Repositories
{
	public interface IVistoriadorProdutoRepository : IRepository<VistoriadorProduto>, IBaseAtivavelRepository<VistoriadorProduto>
    {
	}
}