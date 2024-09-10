using Differencial.Domain.Entities;
using Differencial.Domain.Filters;

namespace Differencial.Domain.Contracts.Services
{
    public interface ISeguradoraService : IBaseService<Seguradora, SeguradoraFilter>
    {
        Seguradora ObterPorRemetenteSolicitacao(string emailremetentesolicitacao);
 
    }
}