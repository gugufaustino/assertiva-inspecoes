using Differencial.Domain.Contracts.Entities;
using Differencial.Domain.Contracts.Repositories;
using Differencial.Domain.Filters;
using Differencial.IOC;
using Differencial.Repository.Context;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleInjector;
using System;
using System.Threading;
using System.Transactions;

namespace Differencial.Repository.Test
{
    [TestClass]
    public abstract class PersistenceTest
    {
        protected static IOC.IOC _ioc = new IOC.IOC(true);
        private Scope _lifetimeScope;
        private TransactionScope _transactionScope;
        protected static DifferencialContext context;

        [AssemblyInitialize]
        public static void SetUp(TestContext testContext)
        {
            //var scope = _ioc.Container.BeginLifetimeScope();
            //var dbContextFactory = _ioc.GetInstance<IDbContextFactory>();
            //context = dbContextFactory.GetDbContext();
            //if (context.Database.Exists())
            //    context.Database.Delete();

            //context.Database.CreateIfNotExists();
            //new DifferencialIntegrationTestInitializer().InitializeDatabase(context);

            //scope.Dispose();
        }

        [TestInitialize]
        public void TestInitialize()
        {

            _lifetimeScope = _ioc.Container.BeginLifetimeScope();

            var dbContextFactory = _ioc.GetInstance<IDbContextFactory>();
            context = dbContextFactory.GetDbContext();

            _transactionScope = new TransactionScope(TransactionScopeOption.RequiresNew,
                new TransactionOptions { Timeout = new TimeSpan(0, 10, 0) });

            Monitor.Enter(IntegrationTestsSynchronization.LockObject);
        }

        [TestCleanup]
        public virtual void TestCleanup()
        {
            Transaction.Current.Rollback();
            _transactionScope.Dispose();

            Monitor.Exit(IntegrationTestsSynchronization.LockObject);
            _lifetimeScope.Dispose();
        }
        
        /// <summary>
        ///     Persistance test helper
        ///     Save and load entity
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <typeparam name="TR"></typeparam>
        /// <typeparam name="TF"></typeparam>
        /// <param name="entity">Entity</param>
        /// <param name="repository"></param>
        /// <param name="codigoPessoaLogada"></param>
        protected T SaveAndLoadEntity<T, TR, TF>(T entity, TR repository, int codigoPessoaLogada)
            where T : class, IEntity
            where TF : Filter
            where TR : IRepository<T, TF>
        {
            repository.Add( entity);
            context.SaveChanges();

            int id = entity.Id;

            var fromDb = repository.GetById(id);
            return fromDb;
        }

        /// <summary>
        ///     Persistance test helper
        ///     Save and load entity
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <param name="entity">Entity</param>
        /// <param name="repository"></param>
        /// <param name="codigoPessoaLogada"></param>
        protected T SaveAndLoadEntity<T, F>(T entity, IRepository<T, F> repository, int codigoPessoaLogada)
            where T : class, IEntity
            where F : Filter, new()
        {
            repository.Add( entity);
            context.SaveChanges();

            int id = entity.Id;

            var fromDb = repository.GetById(id);
            return fromDb;
        }

        /// <summary>
        ///     Persistance test helper
        ///     Edit and load entity
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <typeparam name="TR"></typeparam>
        /// <typeparam name="TF"></typeparam>
        /// <param name="entity">Entity</param>
        /// <param name="repository"></param>
        /// <param name="codigoPessoaLogada"></param>
        protected T EditAndLoadEntity<T, TR, TF>(T entity, TR repository, int codigoPessoaLogada)
            where T : class, IEntity
            where TF : Filter
            where TR : IRepository<T, TF>
        {
            repository.Update( entity);
            context.SaveChanges();

            int id = entity.Id;

            var fromDb = repository.GetById(id);
            return fromDb;
        }

        /// <summary>
        ///     Persistance test helper
        ///     Edit and load entity
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <param name="entity">Entity</param>
        /// <param name="repository"></param>
        /// <param name="codigoPessoaLogada"></param>
        protected T EditAndLoadEntity<T, F>(T entity, IRepository<T, F> repository, int codigoPessoaLogada)
            where T : class, IEntity
            where F : Filter, new()
        {
            repository.Update( entity);
            context.SaveChanges();

            int id = entity.Id;

            var fromDb = repository.GetById(id);
            return fromDb;
        }

        /// <summary>
        ///     Persistance test helper
        ///     Delete and load entity
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <typeparam name="TR">Repository tyoe</typeparam>
        /// <typeparam name="TF">Filter type</typeparam>        
        /// <param name="id">Id</param>
        /// <param name="repository">Repository</param>
        /// <param name="codigoPessoaLogada">Codigo pessoa logada</param>
        protected T DeleteAndLoadEntity<T, TR, TF>(int id, TR repository, int codigoPessoaLogada)
            where T : class, IEntity
            where TF : Filter
            where TR : IRepository<T, TF>
        {
            var entity = repository.GetById(id);
            repository.Update( entity);

            context.SaveChanges();

            var fromDb = repository.GetById(id);
            return fromDb;
        }

        /// <summary>
        ///     Persistance test helper
        ///     Delete and load entity
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <typeparam name="TR">Repository tyoe</typeparam>
        /// <typeparam name="TF">Filter type</typeparam>        
        /// <param name="id">Id</param>
        /// <param name="repository">Repository</param>
        /// <param name="codigoPessoaLogada">Codigo pessoa logada</param>
        protected T DeleteAndLoadEntity<T, F>(int id, IRepository<T, F> repository, int codigoPessoaLogada)
            where T : class, IEntity
            where F : Filter, new()
        {
            var entity = repository.GetById(id);
            repository.Update( entity);

            context.SaveChanges();

            var fromDb = repository.GetById(id);
            return fromDb;
        }


        /// <summary>
        ///     Logical exclusion test helper
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        protected T LoadEntity<T>(int id) where T : class, IEntity
        {
            var fromDb = context.Set<T>().Find(id);
            return fromDb;
        }
    }

    public static class IntegrationTestsSynchronization
    {
        public static readonly object LockObject = new object();
    }
}
