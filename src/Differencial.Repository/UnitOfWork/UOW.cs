using Differencial.Domain.Contracts.Infra;
using Differencial.Domain.UOW;
using Differencial.Repository.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;

using System.ComponentModel.DataAnnotations;
using System.Drawing.Printing;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;

namespace Differencial.Repository.UnitOfWork
{
	public class UnitOfWork : IDisposable, IUnitOfWork
	{

		private readonly IDifferencialContext _db;
		private readonly ILog Log;
		private IDbContextTransaction _transaction;
		private bool disposed = false;


		public UnitOfWork(IDbContextFactory dbContextFactory, IUsuarioService usuario, ILog log)
		{
			_db = dbContextFactory.GetDbContext();
			this.Log = log;
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

		public void AppSaveChanges(int usuarioaplicacao)
		{
			try
			{
				_db.AppSaveChanges(usuarioaplicacao);

			}
			catch (DbUpdateException dbUpEx)
			{
				if (dbUpEx.InnerException != null)
				{
					// Extraindo mensagem de erro
					var message = dbUpEx.InnerException.Message;
					Log.Registrar(dbUpEx, message, TipoLogEnum.Erro);
					throw new Domain.Exceptions.ValidationException(message);
				}
				else
					throw;
			}
			catch (ValidationException validationEx)
			{
				var validationErrors = string.Join(Environment.NewLine, validationEx.ValidationResult.MemberNames);
				Log.Registrar(validationEx, validationErrors, TipoLogEnum.Erro);
				throw new Domain.Exceptions.ValidationException(validationErrors);
			}
		}

		public async Task AppSaveChangesAsync(int usuarioaplicacao)
		{
			try
			{
				await _db.AppSaveChangesAsync(usuarioaplicacao);
			}
			catch (DbUpdateException dbUpEx)
			{
				if (dbUpEx.InnerException != null)
				{
					// Extraindo a mensagem de erro
					var message = dbUpEx.InnerException.Message;
					Log.Registrar(dbUpEx, message, TipoLogEnum.Erro);
					throw new Domain.Exceptions.ValidationException(message);
				}
				else
					throw;

			}
			catch (ValidationException validationEx)
			{
				var validationErrors = string.Join(Environment.NewLine, validationEx.ValidationResult.MemberNames);
				Log.Registrar(validationEx, validationErrors, TipoLogEnum.Erro);
				throw new Domain.Exceptions.ValidationException(validationErrors);
			}
		}

		public IDbContextTransaction GetTransactionAlive()
		{
			return _transaction;
		}

		#region Dispose



		protected virtual void Dispose(bool disposing)
		{
			if (!disposed)
			{
				if (disposing)
				{
					if (_transaction != null)
						_transaction = null;
					_db.Dispose();
				}
			}
			disposed = true;
		}
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		#endregion
	}
}