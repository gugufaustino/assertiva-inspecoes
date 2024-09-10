using Differencial.Domain.Contracts.Services;
using Differencial.Domain.Entities;
using Differencial.Domain.Util.ExtensionMethods;
using Differencial.Infra;
using Differencial.Service.Test.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;


namespace Differencial.Service.Test
{
    /// <summary>
    /// Eu como gerente posso gerar uma nova inspeção a partir de uma inspeção selecionada, esta que por sua vez terá parte dos dados da solicitação original.
    /// </summary>
    [TestClass]
    public class SolicitacaoReinspecaoTest : BaseServiceTest<ISolicitacaoService>
    {
        [TestInitialize]
        public void SolicitacaoReInspecaoInitialize()
        {
            AutenticarUsuario(2);
        }

        /// <summary>
        /// A solicitacao deve atender ao WF como uma nova solicitação(instancia)
        /// </summary>
        [TestMethod]
        [DataTestMethod]
        [DataRow(0)]
        public void GerarEmElaboracaoMovimentoWF(int id)
        {
            Solicitacao clone = null;
            Solicitacao original = null;
            //arrange
            if (id == 0)
            {
                var tmpSol = _service.ListarEmTramitacao().FirstOrDefault();
                id = tmpSol.IsNull() ? 0 : tmpSol.Id;
            }

            //act
            original = _service.Buscar(id);
            clone = _service.Reinspecao(original.Id);
            CommitTransaction();

            //assert
            Assert.AreEqual(clone.TpSituacao, Domain.TipoSituacaoProcessoEnum.EmElaboracao);
            Assert.AreEqual(1, clone.MovimentacaoProcesso.Count);
            Assert.AreEqual(clone.MovimentacaoProcesso.First().TipoSituacaoProcesso, Domain.TipoSituacaoProcessoEnum.EmElaboracao);

        }

        /// <summary>
        /// As seguintes entidades devem ser mantidas:
        /// Detalhe da Solic;
        /// Endereco;
        /// Cliente
        /// Coberturas; 
        /// </summary>
        [TestMethod]
        [DataTestMethod]
        [DataRow(0)]
        public void ManterDadosPrincipais(int id)
        {
            Solicitacao reinspecao = null;
            Solicitacao original = null;
            if (id == 0)
            {
                var tmpSol = _service.ListarEmTramitacao().FirstOrDefault();
                id = tmpSol.IsNull() ? 0 : tmpSol.Id;
            }
            original = _service.Buscar(id);
            reinspecao = _service.Reinspecao(original.Id);

            Assert.AreEqual(original.IdSeguradora, reinspecao.IdSeguradora);
            Assert.AreEqual(original.IdProduto, reinspecao.IdProduto);
            Assert.AreEqual(original.IdFilial, reinspecao.IdFilial);
            Assert.AreEqual(original.IdSolicitante, reinspecao.IdSolicitante);


            Assert.AreEqual(original.IndUrgente, reinspecao.IndUrgente);

            Assert.AreEqual(original.CodSeguradora, reinspecao.CodSeguradora);
            Assert.AreEqual(original.Endereco, reinspecao.Endereco);
            Assert.AreEqual(original.Cliente, reinspecao.Cliente);

            for (int i = 0; i < original.Cobertura.Count; i++)
            {
                var cobOriginal = original.Cobertura.ElementAt(i);
                var cobClone = reinspecao.Cobertura.ElementAtOrDefault(i);
                Assert.IsNotNull(cobClone);
                Assert.AreNotEqual(cobOriginal.IdSolicitacao, cobClone.IdSolicitacao);
                Assert.AreEqual(cobOriginal.NomeCobertura, cobClone.NomeCobertura);
                Assert.AreEqual(cobOriginal.VlrCobertura, cobClone.VlrCobertura);
            }
        }

        /// <summary>
        /// As seguintes propriedades dem ser resetadas para valores determinados
        /// Situacao = Em Elaboracao
        /// Codigo Differencial = ""
        /// Nro Cia = ""
        /// 
        /// Nome Filial
        /// Solicitante 
        /// Solicitante Telefone
        /// Solicitante E-mail
        /// Telefone do Corretor
        /// Telefone do Corretor
        /// 
        /// Atividade
        /// Valor Honorário Pré Acordo
        /// Gestao Vistoriador
        /// Gestao Analista
        /// Gestao Relacionamento
        /// Lancamentos Financeiros
        /// Historico
        /// Movimentacao e Atividade
        /// </summary>
        [TestMethod]
        [DataTestMethod]
        [DataRow(0)]
        public void ResetAtributos(int id)
        {
            Solicitacao reinspecao = null;
            Solicitacao original = null;
            if (id == 0)
            {
                var tmpSol = _service.ListarEmTramitacao().FirstOrDefault();
                id = tmpSol.IsNull() ? 0 : tmpSol.Id;
            }
            original = _service.Buscar(id);
            reinspecao = _service.Reinspecao(original.Id);

            Assert.AreNotEqual(original.IdVistoriador, reinspecao.IdVistoriador);
            Assert.IsNull(reinspecao.Vistoriador);

            if (original.IdAnalista.HasValue)
                Assert.AreNotEqual(original.IdAnalista, reinspecao.IdAnalista);

            Assert.IsNull(reinspecao.Analista);
            Assert.AreEqual(0, reinspecao.CodSistemaLegado);
            Assert.AreEqual(0, reinspecao.Agendamento.Count);
            Assert.AreEqual(0, reinspecao.Foto.Count);
            Assert.AreEqual(0, reinspecao.AtividadeProcesso.Count);
            Assert.AreEqual(0, reinspecao.Comunicacao.Count);
            Assert.IsNull(reinspecao.AreaConstruida);
            Assert.IsNull(reinspecao.BlocoConstruido);
            Assert.IsNull(reinspecao.CasaConstruida);
            Assert.IsNull(reinspecao.CustoDeslocamentoAcordado);
            Assert.IsNull(reinspecao.CustoDeslocamentoPrevisto);
            Assert.IsNull(reinspecao.CustoDeslocamentoRealizado);
            Assert.IsNull(reinspecao.CustoTotalAcordado);
            Assert.IsNull(reinspecao.CustoTotalPrevisto);
            Assert.IsNull(reinspecao.CustoTotalRealizado);
            Assert.IsNull(reinspecao.DeslocamentoPrevisto);
            Assert.IsNull(reinspecao.DeslocamentoRealizado);
            Assert.IsNull(reinspecao.DthRelacionamentoAgendaInformada);
            Assert.IsNull(reinspecao.DthVistoriaRealizada);
            Assert.IsNull(reinspecao.IndCustoVistoriaAcordado);
            Assert.IsNull(reinspecao.IndRelacionamentoAgendaInformada);
            Assert.IsNull(reinspecao.IndRelatorioExigenciaMelhoria);
            Assert.IsNull(reinspecao.IndRotaDeVolta);
            Assert.IsNull(reinspecao.IndUrgente);
            Assert.IsNull(reinspecao.LancamentoFinanceiro);
            Assert.IsNull(reinspecao.QtdEquipamento);
            Assert.IsNull(reinspecao.TipoRotaVistoriaPrevista);
            Assert.IsNull(reinspecao.TipoRotaVistoriaRealizada);
            Assert.IsNull(reinspecao.TxtJustificativaAnalistaDefinido);
            Assert.IsNull(reinspecao.TxtJustificativaDeslocamentoRealizado);
            Assert.IsNull(reinspecao.TxtJustificativaVistoriadorDefinido);
            Assert.IsNull(reinspecao.VistoriadorCidadeBase);
            Assert.IsNull(reinspecao.VlrPagamentoVistoria);
            Assert.IsNull(reinspecao.VlrPagamentoVistoriaAcordado);
            Assert.IsNull(reinspecao.VlrQuilometroRodado);
            Assert.IsNull(reinspecao.VlrRiscoSegurado);

        }

        [TestMethod]
        [DataTestMethod]
        [DataRow(0)]
        public void RelacionamentoSolicitacaoOrigem(int id)
        {
            Solicitacao clone = null;
            Solicitacao original = null;
            //arrange
            if (id == 0)
            {
                var tmpSol = _service.ListarEmTramitacao().FirstOrDefault();
                id = tmpSol.IsNull() ? 0 : tmpSol.Id;
            }

            //act
            original = _service.Buscar(id);
            clone = _service.Reinspecao(original.Id);
            CommitTransaction();
            //assert
            Assert.AreEqual(original.Id, clone.IdSolicitacaoOrigemReinspecao);
        }
    }
}
