using System.Data.Common;

namespace Differencial.Repository.Context
{
    using Microsoft.EntityFrameworkCore;
    using System;

    public interface IDbContextFactory : IDisposable
    {
        DifferencialContext GetDbContext();

        void SetInMemoryDbContext(DbConnection connection);
    }

    public sealed class ContextFactory : IDbContextFactory
    {
        private DifferencialContext _context;

        public ContextFactory(DifferencialContext context)
        {
            _context = context;
        }

        public DifferencialContext GetDbContext()
        {
            return _context;
        }

        public void SetInMemoryDbContext(DbConnection connection)
        {
            _context = new DifferencialContext(new DbContextOptions<DifferencialContext>());
        }

        public void Dispose()
        {
            if (_context != null)
            {
                _context.Dispose();
                GC.SuppressFinalize(this);
            }
        }
    }
}