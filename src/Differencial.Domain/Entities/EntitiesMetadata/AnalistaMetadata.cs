using Differencial.Domain.Contracts.Entities;
using System.ComponentModel.DataAnnotations;

namespace Differencial.Domain.EntitiesMetadata
{
    public class AnalistaMetadata : BaseAuditoriaRegistroMetadata, IAtivavel
    {

        [Display(Name = "Analista")]
        public int Id { get; set; }

        [Display(Name = "Situação")]
        public bool IndAtivo { get; set; }
    }
}