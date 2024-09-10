using Differencial.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Differencial.Domain.Contracts.Entities
{
    public interface IContratoInstanciaValorParametro
    {
        int IdProduto { get; set; }
        int? IdContratoLancamentoValor { get; set; }
        decimal? AreaConstruida { get; set; }
        int? BlocoConstruido { get; set; }
        int? CasaConstruida { get; set; }
        int? QtdEquipamento { get; set; }
        decimal? VlrRiscoSegurado { get; set; }
        decimal? VlrHonorarioPreAcordo { get; set; }
        /// <summary>
        /// Quilometragem de deslocamento do vistoriador para realizar, em cima desse atrbuto será foito pagamento da seguradora
        /// </summary>
        decimal? DeslocamentoRealizado { get; set; }
        bool IndRelatorioExigenciaMelhoria { get; set; } 
        Endereco Endereco { get; set; }

    }
}