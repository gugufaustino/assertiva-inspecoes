using Microsoft.VisualStudio.TestTools.UnitTesting;
using Differencial.Domain.Contracts.Repositories;
using Differencial.Test.Asserts;
using System.Linq;

namespace Differencial.Test.Repository
{
	[TestClass]
	public class LaudoRepositoryTest : BaseTest
	{
		[TestMethod]
		public void Repo_Listar_Laudo()
		{
			//Arrange
			var repo = _ioc.GetInstance<ILaudoRepository>();

			//Act
			var listaLaudo = repo.All();

			//Assert
			//LaudoAssert.Asserts();
		}

	}
}
