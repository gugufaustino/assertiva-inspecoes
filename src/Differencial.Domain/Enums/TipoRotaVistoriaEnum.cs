using System.ComponentModel.DataAnnotations;

namespace Differencial.Domain
{
    public enum TipoRotaVistoriaEnum
    {
        [Display(Name = "A Partir da Base", ShortName = "Cidade Base")]
        CidadeBase = 0,

        [Display(Name = "Entre Vistorias", ShortName = "Entre Vistorias")]
        EntreVistoria = 1, 
    }
}
