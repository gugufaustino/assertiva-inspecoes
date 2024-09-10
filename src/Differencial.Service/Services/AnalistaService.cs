using Differencial.Domain.Contracts.Repositories;
using Differencial.Domain.Contracts.Services;
using Differencial.Domain.Entities;
using Differencial.Domain.Filters;
using Differencial.Domain.UOW;
using System.Collections.Generic;

namespace Differencial.Service.Services
{
	public class AnalistaService : Service, IAnalistaService
	{
		IAnalistaRepository _analistaRepositorio;

		public AnalistaService(IUnitOfWork uow, IAnalistaRepository analistaRepositorio)
			: base(uow)
		{
			_analistaRepositorio = analistaRepositorio;
		}

		public IEnumerable<Analista> Listar(AnalistaFilter filtro)
		{
			return TryCatch(() =>
			{
				return _analistaRepositorio.Where(filtro);
			});
		}

		public void Salvar(Analista entidade)
        {
			TryCatch(() =>
			{
				entidade.Validate();

				if (entidade.Id == 0)
					_analistaRepositorio.Add( entidade);
				else
					_analistaRepositorio.Update( entidade);
			});
		}

		public void Excluir(int id)
        {
			TryCatch(() =>
			{
				_analistaRepositorio.Delete( id);
			});
		}
	}
}