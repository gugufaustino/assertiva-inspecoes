using Differencial.Domain.Contracts.Entities;
using System.ComponentModel.DataAnnotations;

namespace Differencial.Domain.EntitiesMetadata
{
    public class VistoriadorMetadata : BaseAuditoriaRegistroMetadata, IAtivavel
    {

        [Display(Name = "Vistoriador")]
        public int Id { get; set; }

        [Display(Name = "Situação")]
        public bool IndAtivo { get; set; }
    }
}