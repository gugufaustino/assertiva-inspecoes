using Microsoft.EntityFrameworkCore.Infrastructure;
using System;

namespace Differencial.Repository.Context
{
    public interface IDifferencialContext: IDisposable
    {
        void SaveChanges(int usuarioaplicacao);

        DatabaseFacade Database { get; }
    }

}