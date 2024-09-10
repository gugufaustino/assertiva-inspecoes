using Microsoft.VisualStudio.TestTools.UnitTesting;
using Differencial.Domain.Entities;

namespace Differencial.Test.Asserts
{
	public static class EnderecoAssert
	{
		public static void Asserts(Endereco expected, Endereco actual)
		{
			Assert.AreEqual(expected.Id, actual.Id);
			Assert.AreEqual(expected.Cep, actual.Cep);
			Assert.AreEqual(expected.Logradouro, actual.Logradouro);
			Assert.AreEqual(expected.Numero, actual.Numero);
			Assert.AreEqual(expected.Complemento, actual.Complemento);
			Assert.AreEqual(expected.Bairro, actual.Bairro);
			Assert.AreEqual(expected.NomeMunicipio, actual.NomeMunicipio);
			Assert.AreEqual(expected.SiglaUf, actual.SiglaUf);
			Assert.AreEqual(expected.Latitude, actual.Latitude);
			Assert.AreEqual(expected.Longitude, actual.Longitude);
		}

	}
}
