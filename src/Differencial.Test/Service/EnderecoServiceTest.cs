using Microsoft.VisualStudio.TestTools.UnitTesting;
using Differencial.Domain.Contracts.Services;
using Differencial.Domain.Entities;
using Differencial.Domain.Filters;
using Differencial.Test.Asserts;
using System.Linq;

namespace Differencial.Test.Service
{
	[TestClass]
	public class EnderecoServiceTest : BaseTest
	{
		[TestMethod]
		public void Serv_Listar_Endereco()
		{
			//Arrange
			var serv = _ioc.GetInstance<IEnderecoService>();

			//Act
			var listaEndereco = serv.Listar(new EnderecoFilter());

			//Assert
			//EnderecoAssert.Asserts();
		}

	}
}
