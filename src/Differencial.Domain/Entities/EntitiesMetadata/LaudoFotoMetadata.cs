using System.ComponentModel.DataAnnotations;

namespace Differencial.Domain.EntitiesMetadata
{
    public class LaudoFotoMetadata : BaseAuditoriaRegistroMetadata
    {

        [Display(Name = "C�digo")]
        public int Id { get; set; }  
      

    }
}