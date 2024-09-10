using Microsoft.VisualStudio.TestTools.UnitTesting;
using Differencial.Domain.Contracts.Repositories;
using Differencial.Test.Asserts;
using System.Linq;

namespace Differencial.Test.Repository
{
	[TestClass]
	public class EnderecoRepositoryTest : BaseTest
	{
		[TestMethod]
		public void Repo_Listar_Endereco()
		{
			//Arrange
			var repo = _ioc.GetInstance<IEnderecoRepository>();

			//Act
			var listaEndereco = repo.All();

			//Assert
			//EnderecoAssert.Asserts();
		}

	}
}
