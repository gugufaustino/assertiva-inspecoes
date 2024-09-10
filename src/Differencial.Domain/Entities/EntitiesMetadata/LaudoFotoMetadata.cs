using System.ComponentModel.DataAnnotations;

namespace Differencial.Domain.EntitiesMetadata
{
    public class LaudoFotoMetadata : BaseAuditoriaRegistroMetadata
    {

        [Display(Name = "Código")]
        public int Id { get; set; }  
      

    }
}