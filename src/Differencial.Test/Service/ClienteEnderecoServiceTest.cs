using Microsoft.VisualStudio.TestTools.UnitTesting;
using Differencial.Domain.Contracts.Services;
using Differencial.Domain.Entities;
using Differencial.Domain.Filters;
using Differencial.Test.Asserts;
using System.Linq;

namespace Differencial.Test.Service
{
	[TestClass]
	public class ClienteEnderecoServiceTest : BaseTest
	{
		[TestMethod]
		public void Serv_Listar_ClienteEndereco()
		{
			//Arrange
			var serv = _ioc.GetInstance<IClienteEnderecoService>();

			//Act
			var listaClienteEndereco = serv.Listar(new ClienteEnderecoFilter());

			//Assert
			//ClienteEnderecoAssert.Asserts();
		}

	}
}
