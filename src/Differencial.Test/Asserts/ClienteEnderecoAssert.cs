using Microsoft.VisualStudio.TestTools.UnitTesting;
using Differencial.Domain.Entities;

namespace Differencial.Test.Asserts
{
	public static class ClienteEnderecoAssert
	{
		public static void Asserts(ClienteEndereco expected, ClienteEndereco actual)
		{
			Assert.AreEqual(expected.Id, actual.Id);
			Assert.AreEqual(expected.IdCliente, actual.IdCliente);
			Assert.AreEqual(expected.IdEndereco, actual.IdEndereco);
		}

	}
}
