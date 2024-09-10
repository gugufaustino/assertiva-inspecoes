using Differencial.Domain.Contracts.Repositories;
using Differencial.Domain.Contracts.Services;
using Differencial.Domain.Entities;
using Differencial.Domain.Filters;
using Differencial.Domain.UOW;
using System.Collections.Generic;

namespace Differencial.Service.Services
{
	public class AnalistaProdutoService : Service, IAnalistaProdutoService
	{
		IAnalistaProdutoRepository _analistaProdutoRepositorio;

		public AnalistaProdutoService(IUnitOfWork uow, IAnalistaProdutoRepository analistaProdutoRepositorio)
			: base(uow)
		{
			_analistaProdutoRepositorio = analistaProdutoRepositorio;
		}

		public IEnumerable<AnalistaProduto> Listar(AnalistaProdutoFilter filtro)
		{
			return TryCatch(() =>
			{
				return _analistaProdutoRepositorio.Where(filtro);
			});
		}

		public void Salvar(int codigoUsuarioLogado, AnalistaProduto entidade)
		{
			TryCatch(() =>
			{
				entidade.Validate();

				if (entidade.Id == 0)
					_analistaProdutoRepositorio.Add( entidade);
				else
					_analistaProdutoRepositorio.Update( entidade);
			});
		}

		public void Excluir(int codigoUsuarioLogado, int id)
		{
			TryCatch(() =>
			{
				_analistaProdutoRepositorio.Delete( id);
			});
		}
	}
}