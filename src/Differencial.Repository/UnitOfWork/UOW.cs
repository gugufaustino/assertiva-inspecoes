using Differencial.Domain.Contracts.Infra;
using Differencial.Domain.UOW;
using Differencial.Repository.Context;
using Microsoft.EntityFrameworkCore.Storage;
using System;
//using Microsoft.EntityFrameworkCore;

namespace Differencial.Repository
{
    public class UnitOfWork : IDisposable, IUnitOfWork
    {
     
        private readonly IDifferencialContext _db;
     
        private IDbContextTransaction _transaction;
        private bool disposed = false;
        

        public UnitOfWork(IDbContextFactory dbContextFactory, IUsuarioService usuario)
        {
            _db = dbContextFactory.GetDbContext();
        }

        public IDbContextTransaction BeginTransaction()
        {
            
            //Entity não suporta transações paralelas
            if (_transaction != null || _db.Database.CurrentTransaction != null)
            {
#if Debug
                throw new NotSupportedException("Duas transactions foram abertas no Entity Framework ao mesmo tempo, cheque o uso do argumento [CommitedTransaction] ou se algo está abrindo uma transaction no contexto atual (Request atual) sem usar o UnitOfWork. Isso deve ser corrigido. Dica: Se um metodo da controller chama outro metodo de alguma controller ou da mesma, e os dois estão marcados com o atributo, haverá duas tentivas de abrir uma transaction.");
#else
                throw new NotSupportedException("transactions duplicada");
#endif
            }
            
            _transaction = _db.Database.BeginTransaction();
            return _transaction;
        }
        
        public void RollbackTransaction()
        {
            if (_transaction != null)
                _transaction.Rollback();
        }

        public void CommitTransaction()
        {
            if (_transaction != null)
                _transaction.Commit();
        }

        public void SaveChanges(int usuarioaplicacao)
        {
            _db.SaveChanges(usuarioaplicacao);
        }

        public IDbContextTransaction GetTransactionAtiva()
        {
            return _transaction;
        }

        #region Dispose

     

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    if (_transaction != null)
                        _transaction = null;
                    _db.Dispose();
                }
            }
            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}