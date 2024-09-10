using Microsoft.VisualStudio.TestTools.UnitTesting;
using Differencial.Domain.Contracts.Repositories;
using Differencial.Test.Asserts;
using System.Linq;

namespace Differencial.Test.Repository
{
	[TestClass]
	public class LaudoFotoRepositoryTest : BaseTest
	{
		[TestMethod]
		public void Repo_Listar_LaudoFoto()
		{
			//Arrange
			var repo = _ioc.GetInstance<ILaudoFotoRepository>();

			//Act
			var listaLaudoFoto = repo.All();

			//Assert
			//LaudoFotoAssert.Asserts();
		}

	}
}
