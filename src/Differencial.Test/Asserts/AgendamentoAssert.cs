using Microsoft.VisualStudio.TestTools.UnitTesting;
using Differencial.Domain.Entities;

namespace Differencial.Test.Asserts
{
	public static class AgendamentoAssert
	{
		public static void Asserts(Agendamento expected, Agendamento actual)
		{
			Assert.AreEqual(expected.Id, actual.Id);
			Assert.AreEqual(expected.IdSolicitacao, actual.IdSolicitacao);
			Assert.AreEqual(expected.IdVistoriador, actual.IdVistoriador);
			Assert.AreEqual(expected.DthAgendamento, actual.DthAgendamento);
			Assert.AreEqual(expected.IndCancelado, actual.IndCancelado);
		}

	}
}
