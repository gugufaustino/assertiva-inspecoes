using System;
using System.ComponentModel.DataAnnotations;

namespace Differencial.Domain.EntitiesMetadata
{
    public class SolicitacaoMetadata : BaseAuditoriaRegistroMetadata
    {

        [Display(Name = "C�digo Solicita��o", ShortName = "C�digo")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Produto")]
        public int IdProduto { get; set; }

        [Required]
        [Display(Name = "Seguradora")]
        public int IdSeguradora { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "N� CIA")]
        public string CodSeguradora { get; set; }

        [Required]
        [Display(Name = "C�digo Sistema Differencial", ShortName = "C�digo Differencial")]
        public int CodSistemaLegado { get; set; }

        [MaxLength(1000)]
        [Display(Name = "Informa��es Adicionais")]
        public string TxtInformacoesAdicionais { get; set; }

        [MaxLength(250)]
        [Display(Name = "Nome do Corretor")]
        public string CorretorNome { get; set; }

        [MaxLength(13)]
        [Display(Name = "Telefone do Corretor")]
        public string CorretorTelefone { get; set; }

        [Display(Name = "Situa��o")]
        public TipoSituacaoProcessoEnum? TpSituacao { get; set; }

        [Display(Name = "Vistoriador Definido", ShortName = "Vistoriador")]
        public int IdVistoriador { get; set; }

        [MaxLength(1000)]
        [Display(Name = "Justificativa pela defini��o", ShortName = "Justificativa", Description = "Justificativa por definir um vistoriador diferente do sugerido pelo sistema.")]
        public string TxtJustificativaVistoriadorDefinido { get; set; }


        [Display(Name = "Tipo de Rota Prevista" , ShortName = "Tipo Rota", Description = "Sistema avalia a quilometragem do deslocamento, podendo ser a partir do endere�o base ou da vitoria anterior quando realizado agendamento.")]
        public TipoRotaVistoriaEnum? TipoRotaVistoriaPrevista { get; set; }

        [Display(Name = "Deslocamento Previsto (km)", ShortName = "Previsto (km)", Description = "Quantidade de quilometros prevista para deslocamento at� o local da vistoria.")]
        public decimal? DeslocamentoPrevisto { get; set; }

        [Display(Name = "Custo Deslocamento Previsto (R$)", ShortName = "Previsto (R$)", Description = "Valor do deslocamento previsto, sistema calcula o deslocamento pelo valor do quilometro do vistoriador.")]
        public decimal? CustoDeslocamentoPrevisto { get; set; }

        [Display(Name = "Custo Total Previsto (R$)", ShortName = "Total Previsto (R$)", Description = "Valor total previsto,  sistema soma o custo de deslocamento mais o valor do honor�rio do vistoriador.")]
        public decimal? CustoTotalPrevisto { get; set; }


        [Display(Name = "Tipo de Rota Realizada", ShortName = "Tipo Rota", Description = "Informada pelo vistoriador na presta��o de contas, podendo ser a partir do endere�o base ou da vitoria anterior.")]
        public TipoRotaVistoriaEnum? TipoRotaVistoriaRealizada { get; set; }

        [Display(Name = "Deslocamento Realizado (km)", ShortName = "Realizado (km)", Description = "Quantidade de quilometros informado pelo vistoriador na presta��o de contas.")]
        public decimal? DeslocamentoRealizado { get; set; }

        [Display(Name = "Custo Deslocamento Realizado (R$)", ShortName = "Realizado (R$)", Description = "Valor do deslocamento calculado quando vistoriador realizar a presta��o de contas.")]
        public decimal? CustoDeslocamentoRealizado { get; set; }

        [Display(Name = "Custo Total Realizado (R$)", ShortName = "Total Realizado (R$)", Description = "Valor total realizado, soma pelo sistema do custo de deslocamento mais o valor do honor�rio quando informado a quilometragem pelo vistoriador.")]
        public decimal? CustoTotalRealizado { get; set; }


        [Display(Name = "Custo Deslocamento Pre-Acordado (R$)", ShortName  = "Deslocamento Pr�-Acordo (R$)", Description = "Custo de quando h� um acordo pr�vio de quanto ser� pago pelo deslocamento.")]
        public decimal? CustoDeslocamentoAcordado { get; set; }

        [Display(Name = "Custo Pagamento Pre-Acordado (R$)", ShortName = "Honor�rios Pr�-Acordado (R$)", Description = "Valor do honor�rio do servi�o quando h� um acordo previo de quanto ser� pago para a vistoria executada.")]
        public decimal? VlrPagamentoVistoriaAcordado { get; set; }

        [Display(Name = "Custo Total Pre-Acordado (R$)", ShortName = "Total Pr�-Acordado (R$)", Description = "Valor total do Pr�-Acordado, soma do valor do quilometro pr�-acordado multiplicado pelo quilometro realizado e acrescido dos honor�rios pr�-acordado do vistoriador.")]
        public decimal? CustoTotalAcordado { get; set; }


        [Display(Name = "Valor do Honor�rio (R$)", ShortName = "Honor�rios (R$)", Description = "Valor pago pelo honor�rio do servi�o de vistoria prestado.")]
        public decimal? VlrPagamentoVistoria { get; set; }

        [Display(Name = "Valor do Quilometro (R$)", ShortName = "Quilometro (R$)", Description = "Valor pago pelo quilometro rodado para executar a vistoria.")]
        public decimal? VlrQuilometroRodado { get; set; }

        [Display(Name = "Cidade Base")]
        public string VistoriadorCidadeBase { get; set; }


        [Display(Name = "Justificativa Quilometragem Realizada")]
        public string TxtJustificativaDeslocamentoRealizado { get; set; }

        [Display(Name = "Vistoria Com Custo Pr�-Acordado" , Description = "Indica quando h� um acordo previo de valores a ser pago para a vistoria executada.")]
        public bool IndCustoVistoriaAcordado { get; set; }

        [Display(Name = "Data e Hora Vistoria Realizada")]
        public DateTime DthVistoriaRealizada { get; set; }


        [Display(Name = "Solicitante Nome")]
        public string SolicitanteNome { get; set; }

        [MaxLength(13)]
        [Display(Name = "Solicitante Telefone")]
        public string SolicitanteTelefone { get; set; }

        [MaxLength(250)]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Solicitante E-mail")]
        public string SolicitanteEmail { get; set; }

        [Display(Name = "Analista Definido", ShortName = "Analista")]
        public int IdAnalista { get; set; }

        [MaxLength(1000)]
        [Display(Name = "Justificativa pela defini��o", ShortName = "Justificativa", Description = "Justificativa por definir um analista diferente do sugerido pelo sistema.")]
        public string TxtJustificativaAnalistaDefinido { get; set; }

        [Display(Name = "Nome Operador", ShortName = "Operador")]
        public string NomeOperadorAgendaInformada { get; set; }

        [Display(Name = "Data e Hora Realizado")]
        public DateTime? DthRelacionamentoAgendaInformada { get; set; }

        [Display(Name = "Forma de Notifica��o Realizado")]
        public TipoNotificacaoEnum? TipoNotificacaoAgendaInformada { get; set; }
         
        [Display(Name = "Valor do Risco")]
        public decimal? VlrRiscoSegurado { get; set; }
 
        [Display(Name = "Valor Honor�rio Pr� Acordo")]
        public decimal? VlrHonorarioPreAcordo { get; set; }

        [Display(Name = "Solicita��o Urgente", ShortName = "Urgente", Description = "Indica quando � uma solicita��o com prioridade de urg�ncia e este registro � destacado na listas")]
        public bool IndUrgente { get; set; }

        [Display(Name = "Nome Filial", ShortName = "Filial")]
        public int IdFilial { get; set; }

    }
}