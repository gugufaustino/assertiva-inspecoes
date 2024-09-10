using Microsoft.VisualStudio.TestTools.UnitTesting;
using Differencial.Domain.Contracts.Repositories;
using Differencial.Test.Asserts;
using System.Linq;

namespace Differencial.Test.Repository
{
	[TestClass]
	public class ClienteRepositoryTest : BaseTest
	{
		[TestMethod]
		public void Repo_Listar_Cliente()
		{
			//Arrange
			var repo = _ioc.GetInstance<IClienteRepository>();

			//Act
			var listaCliente = repo.All();

			//Assert
			//ClienteAssert.Asserts();
		}

	}
}
