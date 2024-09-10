using Differencial.Domain.Contracts.Entities;
using Differencial.Domain.Resources;
using Differencial.Domain.Util.ExtensionMethods;
using Differencial.Domain.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using Differencial.Domain.EntitiesMetadata;
using Microsoft.AspNetCore.Mvc;    
using System.Collections.Generic;
using System.Linq; 

namespace Differencial.Domain.Entities
{
    [Serializable]
    [ModelMetadataType(typeof(SolicitacaoMetadata))]
    public class Solicitacao : IEntity, IEndereco, IWorkFlowInstanciaProcesso, IContratoInstanciaValorParametro, IClonavel<Solicitacao>, INotificacaoProcesso
    {

        public Solicitacao()
        {
            Agendamento = new HashSet<Agendamento>();
            Foto = new HashSet<ArquivoAnexo>();
            Cobertura = new HashSet<Cobertura>();
            AtividadeProcesso = new HashSet<AtividadeProcesso>();
            MovimentacaoProcesso = new HashSet<MovimentacaoProcesso>();
            LancamentoFinanceiro = new HashSet<LancamentoFinanceiro>();
            Comunicacao = new HashSet<Comunicacao>();
        }

        [Column("Id")]
        public int Id { get; set; }

        [Column("DtCadastro")]
        public DateTime DataCadastro { get; set; }

        [Column("DtModificacao")]
        public DateTime DataModificacao { get; set; }

        [Column("IdOperadorCadastro")]
        public int IdOperadorCadastro { get; set; }

        [Column("IdOperadorModificacao")]
        public int IdOperadorModificacao { get; set; }

        [Column("IdSeguradora")]
        public int IdSeguradora { get; set; }

        [Column("IdProduto")]
        public int IdProduto { get; set; }
        
        [Column("IdContratoLancamentoValor")]
        public int? IdContratoLancamentoValor { get; set; }

        [Column("IdVistoriador")]
        public int? IdVistoriador { get; set; }

        [Column("IdCliente")]
        public int? IdCliente { get; set; }

        [Column("IdEnderecoCliente")]
        public int IdEnderecoCliente { get; set; }

        [Column("CodSeguradora")]
        public string CodSeguradora { get; set; }

        [Column("CodSistemaLegado")]
        public int CodSistemaLegado { get; set; }

        [Column("TpSituacao")]
        public TipoSituacaoProcessoEnum? TpSituacao { get; set; }

        [Column("IdAnalista")]
        public int? IdAnalista { get; set; }

        [Column("TxtInformacoesAdicionais")]
        public string TxtInformacoesAdicionais { get; set; }


        [Column("IdSolicitante")]
        public int? IdSolicitante { get; set; }

        [Column("CorretorNome")]
        public string CorretorNome { get; set; }

        [Column("CorretorTelefone")]
        public string CorretorTelefone { get; set; }


        [Column("TxtJustificativaVistoriadorDefinido")]
        public string TxtJustificativaVistoriadorDefinido { get; set; }


        [Column("VlrPagamentoVistoria")]
        public decimal? VlrPagamentoVistoria { get; set; }

        [Column("VlrQuilometroRodado")]
        public decimal? VlrQuilometroRodado { get; set; }


        [Column("TipoRotaVistoriaPrevista")]
        public TipoRotaVistoriaEnum? TipoRotaVistoriaPrevista { get; set; }

        [Column("DeslocamentoPrevisto")]
        public decimal? DeslocamentoPrevisto { get; set; }

        [Column("CustoDeslocamentoPrevisto")]
        public decimal? CustoDeslocamentoPrevisto { get; set; }

        [Column("CustoTotalPrevisto")]
        public decimal? CustoTotalPrevisto { get; set; }


        [Column("TipoRotaVistoriaRealizada")]
        public TipoRotaVistoriaEnum? TipoRotaVistoriaRealizada { get; set; }

        [Column("DeslocamentoRealizado")]
        public decimal? DeslocamentoRealizado { get; set; }

        [Column("CustoDeslocamentoRealizado")]
        public decimal? CustoDeslocamentoRealizado { get; set; }

        [Column("CustoTotalRealizado")]
        public decimal? CustoTotalRealizado { get; set; }


        [Column("CustoDeslocamentoAcordado")]
        public decimal? CustoDeslocamentoAcordado { get; set; }

        [Column("VlrPagamentoVistoriaAcordado")]
        public decimal? VlrPagamentoVistoriaAcordado { get; set; }

        [Column("CustoTotalAcordado")]
        public decimal? CustoTotalAcordado { get; set; }

        [Column("SolicitanteNome")]
        public string SolicitanteNome { get; set; }

        [Column("SolicitanteTelefone")]
        public string SolicitanteTelefone { get; set; }

        [Column("SolicitanteEmail")]
        public string SolicitanteEmail { get; set; }

        [Column("VistoriadorCidadeBase")]
        public string VistoriadorCidadeBase { get; set; }

        [Column("TxtJustificativaDeslocamentoRealizado")]
        public string TxtJustificativaDeslocamentoRealizado { get; set; }

        [Column("IndCustoVistoriaAcordado")]
        public bool IndCustoVistoriaAcordado { get; set; }

        [Column("DthVistoriaRealizada")]
        public DateTime? DthVistoriaRealizada { get; set; }

        [Column("TxtJustificativaAnalistaDefinido")]
        public string TxtJustificativaAnalistaDefinido { get; set; }

        [Column("IndRelacionamentoAgendaInformada")]
        public bool IndRelacionamentoAgendaInformada { get; set; }

        [Column("NomeOperadorAgendaInformada")]
        public string NomeOperadorAgendaInformada { get; set; }

        [Column("DthRelacionamentoAgendaInformada")]
        public DateTime? DthRelacionamentoAgendaInformada { get; set; }

        [Column("TipoNotificacaoAgendaInformada")]
        public TipoNotificacaoEnum? TipoNotificacaoAgendaInformada { get; set; }

        [Column("AreaContruida")]
        public decimal? AreaConstruida { get; set; }

        [Column("BlocoContruido")]
        public int? BlocoConstruido { get; set; }

        [Column("CasaConstruida")]
        public int? CasaConstruida { get; set; }

        [Column("VlrRiscoSegurado")]
        public decimal? VlrRiscoSegurado { get; set; }

        [Column("VlrHonorarioPreAcordo")]
        public decimal? VlrHonorarioPreAcordo { get; set; }

        [Column("QtdEquipamento")]
        public int? QtdEquipamento { get; set; }

        [Column("IndRelatorioExigenciaMelhoria")]
        public bool IndRelatorioExigenciaMelhoria { get; set; }

        [Column("IndRotaDeVolta")]
        public bool IndRotaDeVolta { get; set; }

        [Column("IndUrgente")]
        public bool IndUrgente { get; set; }

        [Column("IdFilial")]
        public int? IdFilial { get; set; }

        [Column("IdSolicitacaoOrigemReinspecao")]
        public int? IdSolicitacaoOrigemReinspecao { get; set; }

        [Column("ControleDthEmailCobrancaVistoria")]
        public DateTime? ControleDthEmailCobrancaVistoria { get; set; }
        
        #region "Relacionamentos"
        [ForeignKey("IdEnderecoCliente")]
        public virtual Endereco Endereco { get; set; }

        [ForeignKey("IdSeguradora")]
        public virtual Seguradora Seguradora { get; set; }

        [ForeignKey("IdProduto")]
        public virtual Produto Produto { get; set; }

        [ForeignKey("IdCliente")]
        public virtual Cliente Cliente { get; set; }

        [ForeignKey("IdVistoriador")]
        public virtual Vistoriador Vistoriador { get; set; }

        [ForeignKey("IdAnalista")]
        public virtual Analista Analista { get; set; }

        [ForeignKey("IdFilial")]
        public virtual Filial Filial { get; set; }

        public virtual ICollection<Agendamento> Agendamento { get; set; }
        public virtual ICollection<ArquivoAnexo> Foto { get; set; }
        public virtual ICollection<Cobertura> Cobertura { get; set; }
        public virtual ICollection<MovimentacaoProcesso> MovimentacaoProcesso { get; set; }
        public virtual ICollection<AtividadeProcesso> AtividadeProcesso { get; set; }
        public virtual ICollection<LancamentoFinanceiro> LancamentoFinanceiro { get; set; } 
        public virtual ICollection<Comunicacao> Comunicacao { get; set; }



        [NotMapped]
        public ICollection<IWorkFlowMovimentacaoProcesso> WorkFlowInstanciaProcesso
        {
            get
            {
                return MovimentacaoProcesso.Cast<IWorkFlowMovimentacaoProcesso>().ToList();
            }

        }

        [ForeignKey("IdOperadorCadastro")]
        public virtual Operador OperadorCadastro { get; set; }

        [ForeignKey("IdOperadorModificacao")]
        public virtual Operador OperadorModificacao { get; set; }

        [Column("IdOperadorApropriado")]
        public int? IdOperadorApropriado { get; set; }

        [ForeignKey("IdOperadorApropriado")]
        public virtual Operador OperadorApropriado { get; set; }

        [ForeignKey("IdSolicitante")]
        public virtual Solicitante Solicitante { get; set; }

 

        [NotMapped]
        public bool? IndCidadeBaseVistoriador
        {
            get
            {
                return this.IdVistoriador.HasValue ?  (this.NomeMunicipioSiglaUf == this.VistoriadorCidadeBase) : (bool?)null;
            }
        }

        [NotMapped]
        public string NomeMunicipioSiglaUf
        {
            get
            {
                return string.Format("{0} ({1})", Endereco.NomeMunicipio, Endereco.SiglaUf);
            }
        }

        #endregion

        // Valida os dados da entidade
        public void Validate()
        {
            var validationResultsManager = new ValidationResultsManager();

            // Required 
            if (IdProduto == 0)
                validationResultsManager.AddValidationResultNotValid(MensagensValidacao.CampoRelacionamentoInvalido, "Solicitação", "Produto");

            if (CodSeguradora.IsNullOrEmpty())
                validationResultsManager.AddValidationResultNotValid(MensagensValidacao.CampoRequeridoInvalido, "Solicitação", "Nº CIA");

            // Optional
            if (CodSeguradora.IsNullOrEmpty() == false && CodSeguradora.Length > 50)
                validationResultsManager.AddValidationResultNotValid(String.Format(MensagensValidacao.CampoTamanhoMaximoInvalido, "Nº CIA", 50));

            if (TxtInformacoesAdicionais.IsNullOrEmpty() == false && TxtInformacoesAdicionais.Length > 1000)
                validationResultsManager.AddValidationResultNotValid(String.Format(MensagensValidacao.CampoTamanhoMaximoInvalido, "Informações Adicionais", 1000));

            //Validações na edição, para não perder dado
            if (Id > 0)
            {
                if (IdCliente == 0)
                    validationResultsManager.AddValidationResultNotValid(MensagensValidacao.CampoRelacionamentoInvalido, "Solicitação", "Cliente");
            }



            if (validationResultsManager.HasError)
                validationResultsManager.ThrowException();
        }

        // Clona os dados da entidade
        public object Clone()
        { 
            return this.ShallowClone();
        }

        public Solicitacao DeepClone()
        {
            return this.DeepCloneBinarySerialization<Solicitacao>();
        }
    }

    public enum CampoOrdenacaoSolicitacao
    {
        Id,
        IdSeguradora,
        IdProduto,
        IdVistoriador,
    }
}