using System.ComponentModel.DataAnnotations;

namespace Differencial.Domain
{
    public enum TipoNotificacaoEnum
    {
        //[Display(Name = "Notificação", ShortName = "")]
        //NotificacaoSistema = 0,

        //[Display(Name = "Work Flow", ShortName = "")]
        //WorkFlowSistema = 1,

        [Display(Name = "E-mail de Sistema Automático", ShortName = "")]
        EmailSistemaAuto = 3,


        [Display(Name = "E-mail Externo", ShortName = "")]
        EmailExterno = 4,

        [Display(Name = "Sistema da Seguradora", ShortName = "")]
        FerramentaCliente = 5,
    }
}
