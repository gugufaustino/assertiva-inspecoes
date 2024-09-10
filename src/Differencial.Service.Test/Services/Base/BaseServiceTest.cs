using Differencial.Domain.Contracts.Infra;
using Differencial.Domain.Contracts.Services;
using Differencial.Infra;
using SimpleInjector;
using System;

namespace Differencial.Service.Test.Services
{
    public class BaseServiceTest<T> : IDisposable
       where T : class
    {
        protected static IOC.IOC _ioc = new IOC.IOC(true);
        protected Scope _lifetimeScope;
        public T _service;
        private IUsuarioService _usuarioService;
        private IOperadorService _operadorService;

        public BaseServiceTest()
        {
            AutoMapperConfig.RegisterAutoMapper();

            _lifetimeScope = _ioc.Container.BeginLifetimeScope();
            //   _service = _ioc.GetInstance<T>();
            _service = _lifetimeScope.GetInstance<T>();
            //TODO Ver melhor sobre essa contrução
        }

        protected void AutenticarUsuario(int idOperador)
        {
            _usuarioService = _ioc.GetInstance<IUsuarioService>(); 
            _operadorService = _ioc.GetInstance<IOperadorService>(); 
            _usuarioService.Autenticar(_operadorService.Buscar(idOperador)); 
        }

        protected void CommitTransaction()
        {
            var unitOfWork = _ioc.GetInstance<Domain.UOW.IUnitOfWork>();             
            unitOfWork.Commit(_usuarioService.Id);             
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects). 
                    // _lifetimeScope.Dispose();
                    AutoMapperConfig.ResetAutoMapper();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~BaseTest() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            //GC.SuppressFinalize(this);
        }
        #endregion
    }
}
