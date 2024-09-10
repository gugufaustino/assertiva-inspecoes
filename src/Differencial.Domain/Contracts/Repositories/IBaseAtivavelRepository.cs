namespace Differencial.Domain.Contracts.Repositories
{
    public interface IBaseAtivavelRepository<T>
         where T : class
    {
        //void Ativar(T entity);

        void Ativar(int Id); 

        //void Desativar(T entity);

        void Desativar(int Id);
    }
}
