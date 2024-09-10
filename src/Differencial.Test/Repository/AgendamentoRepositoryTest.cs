using Microsoft.VisualStudio.TestTools.UnitTesting;
using Differencial.Domain.Contracts.Repositories;
using Differencial.Test.Asserts;
using System.Linq;

namespace Differencial.Test.Repository
{
	[TestClass]
	public class AgendamentoRepositoryTest : BaseTest
	{
		[TestMethod]
		public void Repo_Listar_Agendamento()
		{
			//Arrange
			var repo = _ioc.GetInstance<IAgendamentoRepository>();

			//Act
			var listaAgendamento = repo.All();

			//Assert
			//AgendamentoAssert.Asserts();
		}

	}
}
