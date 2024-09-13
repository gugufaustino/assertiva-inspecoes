using NLog;
using Differencial.Domain.Exceptions;
using Differencial.Domain.UOW;
using Microsoft.AspNetCore.Http;
using System;
using Differencial.Domain.Resources;
using Differencial.Domain.Contracts.Validation;
using System.Threading.Tasks;

namespace Differencial.Service.Services
{
	public abstract class Service
	{
		protected IUnitOfWork _uow;
		protected Logger _logger = LogManager.GetCurrentClassLogger();
		protected IServicesValidation _serviceValidation;

		public Service(IUnitOfWork uow)
		{
			_uow = uow;
			//  _serviceValidation = (IServicesValidation)new HttpContextAccessor().HttpContext.RequestServices.GetService(typeof(IServicesValidation));

		}

		public T TryCatch<T>(Func<T> func)
		{
			try
			{
				return func();
			}
			catch (NotFoundException)
			{
				throw;
			}
			catch (ValidationException)
			{
				throw;
			}
			catch (Exception ex)
			{
				_logger.Error(ex);
				throw new ServiceException(MensagensErro.ErroServico + ex.Message, ex);
			}
		}

		public void TryCatch(Action action, bool throwException = true)
		{
			try
			{
				action();
			}
			catch (NotFoundException)
			{
				throw;
			}
			catch (ValidationException)
			{
				throw;
			}
			catch (Exception ex)
			{
				_logger.Error(ex);
				if (throwException)
					throw new ServiceException(MensagensErro.ErroServico + Environment.NewLine + ex.Message, ex);
			}
		}

		protected void SaveChange(int idUsuario)
		{
			_uow.SaveChanges(idUsuario);
		}


		public async Task<T> TryCatchAsync<T>(Func<Task<T>> func)
		{
			try
			{
				return await func();
			}
			catch (NotFoundException)
			{
				throw;
			}
			catch (ValidationException)
			{
				throw;
			}
			catch (Exception ex)
			{
				_logger.Error(ex);
				throw new ServiceException(MensagensErro.ErroServico + ex.Message, ex);
			}
		}

		public async Task TryCatchAsync(Func<Task> func)
		{
			try
			{
				await func();
			}
			catch (NotFoundException)
			{
				throw;
			}
			catch (ValidationException)
			{
				throw;
			}
			catch (Exception ex)
			{
				_logger.Error(ex);
				throw new ServiceException(MensagensErro.ErroServico + ex.Message, ex);
			}
		}
	}
}