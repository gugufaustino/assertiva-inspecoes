using Microsoft.VisualStudio.TestTools.UnitTesting;
using Differencial.Domain.Entities;

namespace Differencial.Test.Asserts
{
	public static class ClienteAssert
	{
		public static void Asserts(Cliente expected, Cliente actual)
		{
			Assert.AreEqual(expected.Id, actual.Id);
			Assert.AreEqual(expected.CpfCnpj, actual.CpfCnpj);
			Assert.AreEqual(expected.ContatoNome, actual.ContatoNome);
			Assert.AreEqual(expected.ContatoTelefone, actual.ContatoTelefone);
			Assert.AreEqual(expected.ContatoOutro, actual.ContatoOutro);
			Assert.AreEqual(expected.AtividadeNome, actual.AtividadeNome);
		}

	}
}
