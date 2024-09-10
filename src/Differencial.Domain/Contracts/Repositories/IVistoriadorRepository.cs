using Differencial.Domain.Entities;
using Differencial.Domain.Filters;
using System;
using System.Collections.Generic;

namespace Differencial.Domain.Contracts.Repositories
{
    public interface IVistoriadorRepository : IRepository<Vistoriador>
    {
        IEnumerable<Vistoriador> ListarVistoriadorPorProduto(int idProduto);

        Vistoriador Buscar(int idVistoriador);
        IEnumerable<Vistoriador> ListarVistoriadorOperador(List<int> lstIdOperadores);
    }
}