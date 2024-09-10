using Differencial.Domain.Annotation;
using System.ComponentModel.DataAnnotations;

namespace Differencial.Domain
{
    public enum TipoAtividadeEnum
    {
        [Atividade( IdSituacaoProcessoOrigem = 2 ,Name = "Definir Vistoriador", ShortName = "Def. Vistoriador")]
        DefinirVistoriador = 1,

        [Atividade(IdSituacaoProcessoOrigem = 2, Name = "Definir Analista", ShortName = "Def. Analista")]
        DefinirAnalista = 2,

        [Atividade(IdSituacaoProcessoOrigem = 4, Name = "Agendamento", ShortName = "Agendar" )]
        Agendamento = 3,

        [Atividade(IdSituacaoProcessoOrigem = 4, Name = "Checklist Dados", ShortName = "Checklist Dados", IndAtividadeOpicional = true)]
        ChecklistDados = 4,

        [Atividade(IdSituacaoProcessoOrigem = 4, Name = "Realizar Vistoria", ShortName = "Fotos Vistoria" )]
        RealizarVistoria = 5,

        /*7*/
        [Atividade(IdSituacaoProcessoOrigem = 4, Name = "Elaborar Croqui(Vistoriador)", ShortName = "Elaborar Croqui", IndAtividadeOpicional = true)]
        ElaborarCroquiVistoriador = 7,

        [Atividade(IdSituacaoProcessoOrigem = 4, Name = "Prestacação Conta(Km)", ShortName = "Rota e Km")]
        PrestacaoContaKm = 8,

        /*4*/
        [Atividade(IdSituacaoProcessoOrigem = 7, Name = "Elaborar Quadro de Fotos", ShortName = "Quadro de Fotos" )]
        ElaborarQuadro = 9,

        /*4*/
        [Atividade(IdSituacaoProcessoOrigem = 7 , Name = "Elaborar e Enviar Laudo", ShortName = "Enviar Laudo" )]
        ElaborarEnviarLaudo = 10,

        [Atividade(IdSituacaoProcessoOrigem = 0, Name = "Informar a Seguradora", ShortName = "Inf. Agendamento")]
        InformarAgendamento = 11,

        [Atividade(IdSituacaoProcessoOrigem = 0, Name = "Elaborar Croqui(Analista)", ShortName = "Elaborar Croqui")]
        ElaborarCroquiAnalise = 13,

        [Atividade(IdSituacaoProcessoOrigem = 0, Name = "Registrar Lancamento(Financeiro)", ShortName = "Registrar Lancamento", IndAtividadeOpicional = true)]
        RegistrarLancamentoFinanceiro = 14,
    }
}
