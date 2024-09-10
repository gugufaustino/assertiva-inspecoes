using Differencial.Domain.Contracts.Entities;
using System.ComponentModel.DataAnnotations;

namespace Differencial.Domain.EntitiesMetadata
{
    public class VistoriadorProdutoMetadata : BaseAuditoriaRegistroMetadata, IAtivavel
    {

        [Display(Name = "C�digo")]
        public int Id { get; set; } 

        [Display(Name = "Valor do Honor�rio (R$)", ShortName = "Honor�rios (R$)", Description = "Valor pago pelo honor�rio do servi�o de vistoria prestado.")]
        public decimal? VlrPagamentoVistoria { get; set; }

        [Display(Name = "Valor do Quilometro (R$)", ShortName = "Quilometro (R$)", Description = "Valor pago pelo quilometro rodado para executar a vistoria.")]
        public decimal? VlrQuilometroRodado { get; set; }

        [Display(Name = "Ativo")]
        public bool IndAtivo { get; set; }

    }
}