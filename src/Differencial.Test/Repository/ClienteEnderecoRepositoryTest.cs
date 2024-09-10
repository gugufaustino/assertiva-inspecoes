using Microsoft.VisualStudio.TestTools.UnitTesting;
using Differencial.Domain.Contracts.Repositories;
using Differencial.Test.Asserts;
using System.Linq;

namespace Differencial.Test.Repository
{
	[TestClass]
	public class ClienteEnderecoRepositoryTest : BaseTest
	{
		[TestMethod]
		public void Repo_Listar_ClienteEndereco()
		{
			//Arrange
			var repo = _ioc.GetInstance<IClienteEnderecoRepository>();

			//Act
			var listaClienteEndereco = repo.All();

			//Assert
			//ClienteEnderecoAssert.Asserts();
		}

	}
}
