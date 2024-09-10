using System.Collections.Generic;

namespace Differencial.Domain.Contracts.Services
{
    public interface IBaseService<T, F>
        where T : class
        where F : class
    {
        T Buscar(int id);

        IEnumerable<T> Listar(F filtro);

		void Salvar(T entidade);

		void Excluir(int id);

        void Excluir(int[] ids);
    }
}