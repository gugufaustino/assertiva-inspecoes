using Microsoft.VisualStudio.TestTools.UnitTesting;
using Differencial.Domain.Contracts.Services;
using Differencial.Domain.Entities;
using Differencial.Domain.Filters;
using Differencial.Test.Asserts;
using System.Linq;

namespace Differencial.Test.Service
{
	[TestClass]
	public class AgendamentoServiceTest : BaseTest
	{
		[TestMethod]
		public void Serv_Listar_Agendamento()
		{
			//Arrange
			var serv = _ioc.GetInstance<IAgendamentoService>();

			//Act
			var listaAgendamento = serv.Listar(new AgendamentoFilter());

			//Assert
			//AgendamentoAssert.Asserts();
		}

	}
}
