using System.ComponentModel.DataAnnotations;

namespace Differencial.Domain.EntitiesMetadata
{
    public class LaudoMetadata : BaseAuditoriaRegistroMetadata
    {

        [Display(Name = "C�digo")]
        public int Id { get; set; }  
      

    }
}