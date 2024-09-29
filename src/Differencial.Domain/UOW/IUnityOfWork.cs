

using Microsoft.EntityFrameworkCore.Storage;
using System.Threading.Tasks;

namespace Differencial.Domain.UOW
{
    public interface IUnitOfWork
    {
        IDbContextTransaction BeginTransaction();
        void RollbackTransaction();
        void CommitTransaction();
        void AppSaveChanges(int usuarioaplicacao);
        Task AppSaveChangesAsync(int usuarioaplicacao);
        IDbContextTransaction GetTransactionAlive();
    }
}