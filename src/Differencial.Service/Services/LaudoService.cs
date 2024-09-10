using Differencial.Domain.Contracts.Repositories;
using Differencial.Domain.Contracts.Services;
using Differencial.Domain.Entities;
using Differencial.Domain.Filters;
using Differencial.Domain.UOW;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Differencial.Service.Services
{
	public class LaudoService : Service, ILaudoService
	{
		ILaudoRepository _laudoRepositorio;

		public LaudoService(IUnitOfWork uow, ILaudoRepository laudoRepositorio)
			: base(uow)
		{
			_laudoRepositorio = laudoRepositorio;
		}

		public IEnumerable<Laudo> Listar(LaudoFilter filtro)
		{
			return TryCatch(() =>
			{
				return _laudoRepositorio.Where(filtro);
			});
		}

		public void Salvar(int codigoUsuarioLogado, Laudo entidade)
		{
			TryCatch(() =>
			{
				entidade.Validate();

				if (entidade.Id == 0)
					_laudoRepositorio.Add(entidade);
				else
					_laudoRepositorio.Update(entidade);
			});
		}

		public void Excluir(int codigoUsuarioLogado, int id)
		{
			TryCatch(() =>
			{
				_laudoRepositorio.Delete(id);
			});
		}

       
    }
}