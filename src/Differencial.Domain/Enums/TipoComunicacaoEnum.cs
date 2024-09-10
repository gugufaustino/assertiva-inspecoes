using System.ComponentModel.DataAnnotations;

namespace Differencial.Domain
{
    public enum TipoComunicacaoEnum
    {
        [Display(Name = "Registro")]
        Registro = 0,

        [Display(Name = "Contato com a Seguradora", ShortName = "Contato Seguradora")]
        ContatoSeguradora = 1,

        [Display(Name = "Contato com o Vistoriador", ShortName = "Contato Vistoriador")]
        ContatoVistoriador = 2, 
    }
}
