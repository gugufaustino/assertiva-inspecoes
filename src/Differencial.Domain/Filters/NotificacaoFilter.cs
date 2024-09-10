using Differencial.Domain.Entities;
using System;

namespace Differencial.Domain.Filters
{
	public class NotificacaoFilter: Pagination
	{

		public int? Id { get; set; }
		public string Descricao { get; set; }

		public CampoOrdenacaoNotificacao CampoOrdenacao { get; set; }
	}
}