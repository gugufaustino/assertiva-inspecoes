using Differencial.Domain.Contracts.Repositories;
using Differencial.Domain.Contracts.Services;
using Differencial.Domain.Entities;
using Differencial.Domain.Filters;
using Differencial.Domain.UOW;
using System.Collections.Generic;

namespace Differencial.Service.Services
{
	public class ClienteEnderecoService : Service, IClienteEnderecoService
	{
		IClienteEnderecoRepository _clienteEnderecoRepositorio;

		public ClienteEnderecoService(IUnitOfWork uow, IClienteEnderecoRepository clienteEnderecoRepositorio)
			: base(uow)
		{
			_clienteEnderecoRepositorio = clienteEnderecoRepositorio;
		}

		public IEnumerable<ClienteEndereco> Listar(ClienteEnderecoFilter filtro)
		{
			return TryCatch(() =>
			{
				return _clienteEnderecoRepositorio.Where(filtro);
			});
		}

		public void Salvar(int codigoUsuarioLogado, ClienteEndereco entidade)
		{
			TryCatch(() =>
			{
				entidade.Validate();

				if (entidade.Id == 0)
					_clienteEnderecoRepositorio.Add(entidade);
				else
					_clienteEnderecoRepositorio.Update(entidade);
			});
		}

		public void Excluir(int codigoUsuarioLogado, int id)
		{
			TryCatch(() =>
			{
				_clienteEnderecoRepositorio.Delete(id);
			});
		}
	}
}