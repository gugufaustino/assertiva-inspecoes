using Differencial.Domain.Entities;
using System;

namespace Differencial.Domain.DTO
{
    public class VistoriadorProdutoValorDTO
    {
        public int IdVistoriadorProduto { get; set; }
        public int IdProduto { get; set; }
        public int IdVistoriador { get; set; }
        public int IdContratoLancamento { get; set; }
        public int IdContratoLancamentoValor { get; set; }
        public TipoContratoParametroEnum TipoParametroQuantitativoVariavel { get; set; }
        public string NomeProduto { get; set; }
        public string NomeSeguradora { get; set; }
        public virtual VistoriadorProduto VistoriadorProduto { get; set; }
        public virtual ContratoLancamentoValor ContratoLancamentoValor { get; set; }

        /// <summary>
        /// Chave vitual de quando há lanlamento valor, então os valores atribuidos aqui são exclusivamente de quando há Valor por faixa de valors
        /// a Atribuição acontece na repositório em um método exclusivo para criação dessa "chave virtual"
        /// </summary>
        public KeyVistoriadorProdutoLancamentoDTO KeyVistoriadorProdutoLancamentoValor { get; set; }

    }

    public class KeyVistoriadorProdutoLancamentoDTO
    {         
        public int IdVistoriadorProduto { get; set; }
        public int IdProduto { get; set; }        
        public int IdContratoLancamento { get; set; }
        public int IdContratoLancamentoValor { get; set; }
    }
}
