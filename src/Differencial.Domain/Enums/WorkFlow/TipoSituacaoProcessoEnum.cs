using Differencial.Domain.Annotation;
using Differencial.Domain.Enums.WorkFlow;
using System.ComponentModel.DataAnnotations;

namespace Differencial.Domain
{
    public enum TipoSituacaoProcessoEnum
    {
        [SituacaoProcesso(Name = "Em Elaboração", ShortName = "Elaboração",
        WFPapelAtuante = (int)TipoPapelEnum.Gerente,
        WFTipoAcaoPapelAtuante = [WFTipoAcao.Apropriar],
        WFTipoAcaoOperadorApropriado = [WFTipoAcao.Gravar, WFTipoAcao.Enviar])]
        EmElaboracao = 1,


        [SituacaoProcesso(Name = "Apropriado Pelo Gerente", ShortName = "Aprop. Gerente", IndGerarAtividade = true,
        WFPapelAtuante = (int)TipoPapelEnum.Gerente, 
        WFTipoAcaoPapelAtuante = [WFTipoAcao.Apropriar],
        WFTipoAcaoOperadorApropriado = [WFTipoAcao.Gravar, WFTipoAcao.Enviar, WFTipoAcao.Cancelar ])]
        ApropriadoGerente = 2,


        [SituacaoProcesso(Name = "Enviado Para Vistoria", ShortName = "Vistoria", NomeAcao = "Enviar para Vistoria",
        WFPapelAtuante = (int)TipoPapelEnum.Vistoriador,
        WFTipoAcaoPapelAtuante = new[] { WFTipoAcao.Apropriar, WFTipoAcao.Devolver })]
        EnviadoParaVistoria = 3,


        [SituacaoProcesso(Name = "Apropriado Pelo Vistoriador", ShortName = "Leitura Confirmada", IndGerarAtividade = true,
        WFTipoAcaoOperadorApropriado = new[] { WFTipoAcao.Enviar, WFTipoAcao.Devolver })]
        ApropriadoVistoriador = 4,

       
        [SituacaoProcesso(Name = "Devolvido para Gerência", ShortName = "Devolvido", NomeAcao = "Devolver para Gerência",
        WFMotivoAcao = new[] { (int)TipoMotivoEnum.NãoAtuoNaRegiao, (int)TipoMotivoEnum.Sobrecarregado },
        WFPapelAtuante = (int)TipoPapelEnum.Gerente,
        WFTipoAcaoPapelAtuante = new[] { WFTipoAcao.Apropriar },
        WFTipoAcaoOperadorApropriado = new[] { WFTipoAcao.Enviar, WFTipoAcao.Cancelar })]
        DevolvidoParaGerencia = 5,


        [SituacaoProcesso(Name = "Enviado para Análise", ShortName = "Análise", NomeAcao = "Enviar para Análise",
        WFPapelAtuante = (int)TipoPapelEnum.Analista,
        WFTipoAcaoPapelAtuante = new[] { WFTipoAcao.Apropriar })]
        EnviadoParaAnalise = 6,

        [SituacaoProcesso(Name = "Apropriado Pelo Analista", ShortName = "Aprop. Analista", IndGerarAtividade = true,
        WFPapelAtuante = (int)TipoPapelEnum.Analista,
        WFTipoAcaoPapelAtuante = new[] { WFTipoAcao.Apropriar },
        WFTipoAcaoOperadorApropriado = new[] { WFTipoAcao.GravarAtividade, WFTipoAcao.Enviar})]
        ApropriadoPelaAnalise = 7,


        [SituacaoProcesso(Name = "Devolvido para Vistoriador", ShortName = "Vistoriador", NomeAcao = "Devolver para Vistoriador", IndMotivocao = false,
        WFTipoAcaoOperadorApropriado = new[] { WFTipoAcao.GravarAtividade, WFTipoAcao.Enviar, WFTipoAcao.Devolver })]
        DevolvidoParaVistoriador = 8,


        [SituacaoProcesso(Name = "Enviado Para Financeiro", ShortName = "Seguradora", NomeAcao = "Enviar para Financeiro",
        WFPapelAtuante = (int)TipoPapelEnum.Financeiro,
        WFTipoAcaoPapelAtuante = new[] { WFTipoAcao.Apropriar })]
        EnviadoParaFinanceiro = 9,


        [SituacaoProcesso(Name = "Apropriado Pelo Financeiro", ShortName = "Aprop. Financeiro",
        WFPapelAtuante = (int)TipoPapelEnum.Financeiro,
        WFTipoAcaoPapelAtuante = new[] { WFTipoAcao.Apropriar },
        WFTipoAcaoOperadorApropriado = new[] { WFTipoAcao.Concluir })]
        ApropriadoPeloFinanceiro = 10,
         

        [SituacaoProcesso(Name = "Cancelado Pela Gerência", ShortName = "Cancelado" )]
        Cancelado = 11,


        [SituacaoProcesso(Name = "Cancelado Pela Seguradora", ShortName = "Cancelado CIA." )]
        CanceladoPelaSeguradora = 12,


        [SituacaoProcesso(Name = "Em Elaboração Solicitante", ShortName = "Elaboração",
        WFPapelAtuante = (int)TipoPapelEnum.Solicitante,
        WFTipoAcaoPapelAtuante = new[] { WFTipoAcao.Apropriar },
        WFTipoAcaoOperadorApropriado = new[] { WFTipoAcao.Gravar, WFTipoAcao.Enviar })]
        EmElaboracaoSolicitante = 13,

        [SituacaoProcesso(Name = "Enviado Para Gerência", ShortName = "Gerência", NomeAcao = "Enviar para Gerência",
        WFPapelAtuante = (int)TipoPapelEnum.Vistoriador,
        WFTipoAcaoPapelAtuante = new[] { WFTipoAcao.Apropriar })]
        EnviadoParaGerencia = 14,


        [SituacaoProcesso(Name = "Apropriado Pelo Solicitante", ShortName = "Aprop. Solicitante",
        WFPapelAtuante = (int)TipoPapelEnum.Solicitante,
        WFTipoAcaoPapelAtuante = new[] { WFTipoAcao.Apropriar },
        WFTipoAcaoOperadorApropriado = new[] { WFTipoAcao.Gravar, WFTipoAcao.Enviar, WFTipoAcao.Cancelar })]
        ApropriadoPeloSolicitante = 15,

        [SituacaoProcesso(Name = "Devolvido Para Seguradora", ShortName = "Devolvido", NomeAcao = "Devolver para Solicitante",
        WFPapelAtuante = (int)TipoPapelEnum.Solicitante,  WFTipoAcaoPapelAtuante = new[] { WFTipoAcao.Apropriar })]     
        DevolvidoParaSeguradora = 16,

        [SituacaoProcesso(Name = "Concluído", ShortName = "Concluído", NomeAcao = "Concluír")]
        Concluido = 17,
    }
}
