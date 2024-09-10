using Microsoft.VisualStudio.TestTools.UnitTesting;
using Differencial.Domain.Entities;

namespace Differencial.Test.Asserts
{
	public static class LaudoFotoAssert
	{
		public static void Asserts(LaudoFoto expected, LaudoFoto actual)
		{
			Assert.AreEqual(expected.Id, actual.Id);
			Assert.AreEqual(expected.IdLaudo, actual.IdLaudo);
			Assert.AreEqual(expected.IdFoto, actual.IdFoto);
		}

	}
}
