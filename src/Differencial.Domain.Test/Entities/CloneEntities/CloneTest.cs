using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Differencial.Domain.Entities;
using System.Collections.Generic;
namespace Differencial.Domain.Test.Entities.CloneEntities
{
    [TestClass]
    public class CloneTest
    {
        Solicitacao solic;
        [TestInitialize]
        public void CloneInitialize()
        {
            var seguradora55 = new Seguradora { Id = 55, NomeSeguradora = "Seguro Testes" };
            var produto44 = new Produto { Id = 44, NomeProduto = "44teste produto" };
            var agendaDez = new List<Agendamento>() { new Agendamento { IdSolicitacao = 1, DthAgendamento = new DateTime(2018, 12, 5), IndCancelado = false } };
            var operadorVistoriador33 = new Operador { Id = 33, NomeOperador = "Vani", Vistoriador = new Vistoriador { } };
            var endereco22 = new Endereco { Id = 22, SiglaUf = "rs", NomeMunicipio = "Porto", Logradouro = "Carlos Von" };
            var cliente66 = new Cliente { Id = 11, CpfCnpj = "01866798073", NomeRazaoSocial = "Wellington Leal Faustino" };
            solic = new Solicitacao()
            {
                Id = 1,
                Agendamento = agendaDez,
                IdCliente = 11,
                Cliente = cliente66,
                IdSeguradora = seguradora55.Id,
                Seguradora = seguradora55,
                CodSeguradora = "2XX",
                CodSistemaLegado = 2,
               
                Endereco = endereco22,
                IdEnderecoCliente = endereco22.Id,
                IdOperadorApropriado = operadorVistoriador33.Id,
                OperadorApropriado = operadorVistoriador33,
                IdProduto = produto44.Id,
                Produto = produto44,
                IdVistoriador = operadorVistoriador33.Id,
                Vistoriador = operadorVistoriador33.Vistoriador,
                TipoRotaVistoriaPrevista = TipoRotaVistoriaEnum.EntreVistoria,
                TpSituacao = TipoSituacaoProcessoEnum.ApropriadoVistoriador,

                CustoDeslocamentoPrevisto = 200,
                CustoTotalPrevisto = 250,
                DeslocamentoPrevisto = 50,
                VlrPagamentoVistoria = 35,
                VlrQuilometroRodado = 0.55m
            };
        }
        /// <summary>
        /// Objetos complexos(como listas, e objetos) da entidade não podem ser referenciados, devem ser inicializado com valor default, no caso nulo;
        /// </summary>
        [TestMethod]
        public void NewEntityShallowClone()
        {
            //Arrange - CloneInitialize()

            //Act
            Solicitacao solicClone = (Solicitacao)solic.Clone();

            //Assert
            Assert.AreEqual(solic.Id, solicClone.Id);
            solicClone.Id = 0;
            Assert.AreNotEqual(solic.Id, solicClone.Id);          

            Assert.AreEqual(solic.TpSituacao, solicClone.TpSituacao);
            solicClone.TpSituacao = TipoSituacaoProcessoEnum.EmElaboracao;
            Assert.AreNotEqual(solic.TpSituacao, solicClone.TpSituacao);

            Assert.AreEqual(solic.CustoDeslocamentoPrevisto, solicClone.CustoDeslocamentoPrevisto);
            solicClone.CustoDeslocamentoPrevisto = 0;
            Assert.AreNotEqual(solic.CustoDeslocamentoPrevisto, solicClone.CustoDeslocamentoPrevisto);
           
            Assert.IsNull(solicClone.Seguradora);
            Assert.IsNull(solicClone.Produto);
            Assert.IsNull(solicClone.Cliente);

            Assert.AreEqual(0, solicClone.Agendamento.Count);
            Assert.AreEqual(0, solicClone.MovimentacaoProcesso.Count);

        }
        /// <summary>
        /// Objetos comlexos(como listas, e objetos)  da entidade devem ser copiados como novas instancias sem referencia a anterior
        /// </summary>
        [TestMethod]
        public void NewEntityDeepClone()
        {
            //Arrange - CloneInitialize()

            //Act
            Solicitacao solicClone = (Solicitacao)solic.DeepClone();

            //Assert
            Assert.AreEqual(solic.Id, solicClone.Id);
            Assert.AreEqual(solic.Cliente, solicClone.Cliente);


        }
    }
}