using Microsoft.VisualStudio.TestTools.UnitTesting;
using Differencial.Domain.Contracts.Repositories;
using Differencial.Test.Asserts;
using System.Linq;

namespace Differencial.Test.Repository
{
	[TestClass]
	public class FotosRepositoryTest : BaseTest
	{
		[TestMethod]
		public void Repo_Listar_Fotos()
		{
			//Arrange
			var repo = _ioc.GetInstance<IFotosRepository>();

			//Act
			var listaFotos = repo.All();

			//Assert
			//FotosAssert.Asserts();
		}

	}
}
