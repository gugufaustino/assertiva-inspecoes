using Differencial.Domain.Entities;
using System;

namespace Differencial.Domain.Filters
{
	public class NotificacaoOperadorFilter: Pagination
	{

		public int? Id { get; set; }
		public bool? IndLido { get; set; }
		public int? IdNotificacao { get; set; }
		public int? IdOperador { get; set; }

		public CampoOrdenacaoNotificacaoOperador CampoOrdenacao { get; set; }
	}
}