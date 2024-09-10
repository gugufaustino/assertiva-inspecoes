using Microsoft.VisualStudio.TestTools.UnitTesting;
using Differencial.Domain.Contracts.Services;
using Differencial.Domain.Entities;
using Differencial.Domain.Filters;
using Differencial.Test.Asserts;
using System.Linq;

namespace Differencial.Test.Service
{
	[TestClass]
	public class LaudoFotoServiceTest : BaseTest
	{
		[TestMethod]
		public void Serv_Listar_LaudoFoto()
		{
			//Arrange
			var serv = _ioc.GetInstance<ILaudoFotoService>();

			//Act
			var listaLaudoFoto = serv.Listar(new LaudoFotoFilter());

			//Assert
			//LaudoFotoAssert.Asserts();
		}

	}
}
