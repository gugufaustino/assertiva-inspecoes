using Differencial.Domain.Entities;
using System;

namespace Differencial.Domain.Filters
{
	public class AnalistaFilter: Pagination
	{


		public CampoOrdenacaoAnalista CampoOrdenacao { get; set; }
	}
}