using Differencial.Domain.DTO;
using Differencial.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace Differencial.Domain.Contracts.Infra
{
    public interface IUsuarioService : IUsuario
    {
 
        bool Autenticado();
        Task Autenticar(Operador operador);
        Task Remover();
        UsuarioLogadoDTO UsuarioAutenticado { get; }

    }
}
