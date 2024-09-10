using Differencial.Domain.Entities;
using Differencial.Domain.Filters;
using System.Collections.Generic;
using Differencial.Domain.Contracts.Entities;

namespace Differencial.Domain.Contracts.Services
{
    public interface IContratoService
    {
        IEnumerable<Contrato> Listar(ContratoFilter filtro);

        void Salvar(Contrato entidade);

        void Excluir(int id);

        Dictionary<TipoContratoParametroEnum, decimal> GerarLancamentosContrato(IContratoInstanciaValorParametro solicitacao);

        void ValidarContratoLancamentoValorParametro(IContratoInstanciaValorParametro solicitacao, TipoContratoParametroEnum tipoContratoParametroEnum);
        List<TipoContratoParametroEnum> ParametrosObrigatorios(Contrato contrato);

        bool IndicaContratoLancamentoValorRisco(IContratoInstanciaValorParametro contratLancamentoValor, out int? IdContratoLancamentoValor);
    }
}