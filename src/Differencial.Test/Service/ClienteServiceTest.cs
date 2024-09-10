using Microsoft.VisualStudio.TestTools.UnitTesting;
using Differencial.Domain.Contracts.Services;
using Differencial.Domain.Entities;
using Differencial.Domain.Filters;
using Differencial.Test.Asserts;
using System.Linq;

namespace Differencial.Test.Service
{
	[TestClass]
	public class ClienteServiceTest : BaseTest
	{
		[TestMethod]
		public void Serv_Listar_Cliente()
		{
			//Arrange
			var serv = _ioc.GetInstance<IClienteService>();

			//Act
			var listaCliente = serv.Listar(new ClienteFilter());

			//Assert
			//ClienteAssert.Asserts();
		}

	}
}
