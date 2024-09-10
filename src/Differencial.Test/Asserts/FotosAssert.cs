using Microsoft.VisualStudio.TestTools.UnitTesting;
using Differencial.Domain.Entities;

namespace Differencial.Test.Asserts
{
	public static class FotosAssert
	{
		public static void Asserts(Fotos expected, Fotos actual)
		{
			Assert.AreEqual(expected.Id, actual.Id);
			Assert.AreEqual(expected.IdSolicitacao, actual.IdSolicitacao);
		}

	}
}
