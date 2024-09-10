using Microsoft.VisualStudio.TestTools.UnitTesting;
using Differencial.Domain.Contracts.Services;
using Differencial.Domain.Entities;
using Differencial.Domain.Filters;
using Differencial.Test.Asserts;
using System.Linq;

namespace Differencial.Test.Service
{
	[TestClass]
	public class LaudoServiceTest : BaseTest
	{
		[TestMethod]
		public void Serv_Listar_Laudo()
		{
			//Arrange
			var serv = _ioc.GetInstance<ILaudoService>();

			//Act
			var listaLaudo = serv.Listar(new LaudoFilter());

			//Assert
			//LaudoAssert.Asserts();
		}

	}
}
