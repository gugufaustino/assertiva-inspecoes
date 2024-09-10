using Microsoft.VisualStudio.TestTools.UnitTesting;
using Differencial.Domain.Contracts.Services;
using Differencial.Domain.Entities;
using Differencial.Domain.Filters;
using Differencial.Test.Asserts;
using System.Linq;

namespace Differencial.Test.Service
{
	[TestClass]
	public class FotosServiceTest : BaseTest
	{
		[TestMethod]
		public void Serv_Listar_Fotos()
		{
			//Arrange
			var serv = _ioc.GetInstance<IFotosService>();

			//Act
			var listaFotos = serv.Listar(new FotosFilter());

			//Assert
			//FotosAssert.Asserts();
		}

	}
}
