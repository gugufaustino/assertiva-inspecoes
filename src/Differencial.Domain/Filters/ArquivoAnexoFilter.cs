using Differencial.Domain.Entities;
using Differencial.Domain;

namespace Differencial.Domain.Filters
{
    public class ArquivoAnexoFilter : Pagination
    {

        public int? Id { get; set; }

        public int? IdSolicitacao { get; set; }

        public TipoArquivoAnexoEnum? TipoArquivoAnexoEnum { get; set;}

        public CampoOrdenacaoArquivoAnexo CampoOrdenacao { get; set; }
	}
}