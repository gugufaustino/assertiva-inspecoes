using Differencial.Domain.DTO;
using Differencial.Domain.Entities;
using Differencial.Domain.Filters;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Differencial.Domain.Contracts.Repositories
{
    public interface IOperadorRepository : IRepository<Operador>
    {
       IEnumerable<Operador> ListarOperadorCadastro(OperadorFilter filter);
       Task<Operador> BuscarParaEditarView(int id);
       Task<Operador> BuscarParaEditarUpdate(int id);
   
        IEnumerable<Operador> Listar(OperadorFilter filter);
    }
}