using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Threading.Tasks;

namespace Differencial.Repository.Context
{
    public interface IDifferencialContext: IDisposable
    {
        void AppSaveChanges(int usuarioaplicacao);
		Task AppSaveChangesAsync(int usuarioaplicacao);

        DatabaseFacade Database { get; }
    }

}