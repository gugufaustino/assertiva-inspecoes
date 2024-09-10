using Differencial.Domain.Contracts.Entities;
using System.ComponentModel.DataAnnotations;

namespace Differencial.Domain.EntitiesMetadata
{
    public class SeguradoraMetadata : BaseAuditoriaRegistroMetadata, IAtivavel
    {
        [Display(Name = "Código Seguradora", ShortName = "Código")]
        public int  Id { get; set; }
 
        [Required]
        [Display(Name = "Nome Seguradora")]
        public string NomeSeguradora { get; set; }

        [Display(Name = "CNPJ")]
        public string Cnpj { get; set; }

        [Display(Name = "Inscrição")]
        public string Inscricao { get; set; }

        [Display(Name = "Razão Social")]
        public string RazaoSocial { get; set; }

        [Required(ErrorMessage = "'Contábil - Seguradora paga inspeções enviadas - Do dia'. É inválido, o preenchimento desse campo é obrigatório.")]
        [Display(Name = "Do dia", Description = "Seguradora paga inspeções enviadas")]
        public int ContabilInspecoesDiaInicio { get; set; }

        [Required(ErrorMessage = "'Contábil - Seguradora paga inspeções enviadas - Até o dia'. É inválido, o preenchimento desse campo é obrigatório.")]
        [Display(Name = "Até o dia", Description = "Seguradora paga inspeções enviadas")]
        public int ContabilInspecoesDiaFim { get; set; }

        [Required(ErrorMessage = "'Contábil - Differencial paga inspeções - Dia Inspetor'. É inválido, o preenchimento desse campo é obrigatório.")]
        [Display(Name = "Dia Inspetor")]
        public int ContabilInspetorDia { get; set; }

        [Required(ErrorMessage = "'Aba Contábil - Differencial paga inspeções - Dia Differencial'. É inválido, o preenchimento desse campo é obrigatório.")]
        [Display(Name = "Dia Differencial")]
        public int ContabilEmpresaDia { get; set; }



        [Display(Name = "Situação")]
        public bool IndAtivo { get; set; }


        [Display(Name = "Franquia")]
        public int QtdQuilometroFranquia { get; set; }

        [Display(Name = "Valor(R$) Quilometro Excedente")]
        public decimal? VlrQuilometroExcedente { get; set; }


        [Display(Name = "Caixa de Entrada do Gmail", Description = "Habilita o sistema ler a caixa de entrada da Differencial e procurar por solicitações dessa seguradora.")]
        public bool IndIntegracaoSolicitacaoPorEmail { get; set; }

        [Display(Name = "E-mail da seguradora", Description = "E-mail de uso geral do sistema para envio de agendamentos, laudos e também realizar a leitura de solicitação existe integração no Gmail.")]
        public string EmailRemetenteSolicitacao { get; set; }

        [Display(Name = "Agenda enviado para e-mail", Description = "Habilita que o sistema envie automaticamente o e-mail com agendamento realizado na 'aba Gestão - aba Relacionamento - botão Informar Agendamento'.")]
        public bool IndAgendaRepostaPorEmail { get; set; }

        [Display(Name = "Laudo enviado para e-mail", Description = "Habilita que o sistema envie automaticamente o e-mail com laudo anexado na 'aba Analise - aba Laudo - botão Concluir Laudo'.")]
        public bool IndLaudoRepostaPorEmail { get; set; }
 
 

    }
}
