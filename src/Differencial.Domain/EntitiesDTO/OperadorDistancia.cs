using Differencial.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Differencial.Domain.DTO
{
    [ComplexType]
    public class OperadorDistancia
    {
        public int Id { get; set; }
        public string NomeOperador { get; set; }
        public double? DistanciaRaio { get; set; }

        
        public double? DistanciaRota { get; set; }

        [NotMapped]
        public Operador Operador { get; set; }

        [NotMapped]
        public virtual decimal? VlrPagamentoVistoria { get; set; }

        [NotMapped]
        public virtual decimal? VlrQuilometroRodado { get; set; }

        [NotMapped]
        public virtual decimal? VlrTotalQuilometroRodado { get; set; }

        [NotMapped]
        public virtual decimal? VlrTotalQuilometroRodadoMaisPagamentoVistoria { get; set; }

        [NotMapped]
        public bool IndHistoricoMesmaCidade { get; set; }

        [NotMapped]
        public bool IndCidadeBase { get; set; }

        [NotMapped]
        public bool IndSolicitacaoMesmaCidade { get; set; }

        [NotMapped]
        public string NomeMunicipioSiglaUf { get; set; }

        [NotMapped]
        public string UrlVerMapa { get; set; }
    }
}
