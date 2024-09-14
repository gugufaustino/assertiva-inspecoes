

using Microsoft.EntityFrameworkCore.Storage;

namespace Differencial.Domain.UOW
{
    public interface IUnitOfWork
    {
        IDbContextTransaction BeginTransaction();
        void RollbackTransaction();
        void CommitTransaction();
        void AppSaveChanges(int usuarioaplicacao);
        IDbContextTransaction GetTransactionAlive();
    }
}