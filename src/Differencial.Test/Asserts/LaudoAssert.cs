using Microsoft.VisualStudio.TestTools.UnitTesting;
using Differencial.Domain.Entities;

namespace Differencial.Test.Asserts
{
	public static class LaudoAssert
	{
		public static void Asserts(Laudo expected, Laudo actual)
		{
			Assert.AreEqual(expected.Id, actual.Id);
			Assert.AreEqual(expected.IdSolicitacao, actual.IdSolicitacao);
		}

	}
}
