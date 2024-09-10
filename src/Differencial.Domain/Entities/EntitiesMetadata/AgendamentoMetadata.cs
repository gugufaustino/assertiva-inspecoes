using System;
using System.ComponentModel.DataAnnotations;

namespace Differencial.Domain.EntitiesMetadata
{
    public class AgendamentoMetadata : BaseAuditoriaRegistroMetadata
    {

        [Display(Name = "Código Agendamento")]
        public int Id { get; set; }

        [Display(Name = "Data Agendamento")]
        public DateTime? DthAgendamento { get; set; }

        [Display(Name = "Agendamento Cancelado")]
        public bool IndCancelado { get; set; }
    }
}