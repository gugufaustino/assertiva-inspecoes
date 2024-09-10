using System.ComponentModel.DataAnnotations;

namespace Differencial.Domain.EntitiesMetadata
{
    public class ClienteMetadata : BaseAuditoriaRegistroMetadata
    {

        [Display(Name = "C�digo Cliente")]
        public int Id { get; set; }

        
        [MaxLength(14)]
        [Display(Name = "CPF ou CNPJ")]
        public string CpfCnpj { get; set; }

        [Required]
        [MaxLength(250)]
        [Display(Name = "Nome ou Raz�o Social")]
        public string NomeRazaoSocial { get; set; }

        [MaxLength(250)]
        [Display(Name = "Nome para Contato")]
        public string ContatoNome { get; set; }

        [MaxLength(13)]
        [Display(Name = "Telefone de Contato")]
        public string ContatoTelefone { get; set; }

        [MaxLength(250)]
        [Display(Name = "Outro Contato")]
        public string ContatoOutro { get; set; }

        [MaxLength(250)]
        [Display(Name = "Contato Agendamento", Description = "Esse � o contato realizado pelo vistoriador no momento de agendar a inspe��o com o cliente")]
        public string ContatoAgendamento { get; set; }

        [MaxLength(250)]
        [Display(Name = "Atividade")]
        public string AtividadeNome { get; set; }


    }
}