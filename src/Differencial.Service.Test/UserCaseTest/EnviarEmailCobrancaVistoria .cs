using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Differencial.Service.Test.Services;
using Differencial.Domain.Contracts.Services;
using Differencial.Domain.Queries;

namespace Differencial.Service.Test.UserCaseTest
{

    /// <summary>
    /// Eu como sistema tenho que consultar todas solicitações com agendamento atrasado mais de(24h)
    /// e enviar um email de cobrança para cada vistoriador apropriado|enviado da respectiva solicitação.
    /// </summary>
    [TestClass]
    public class EnviarEmailCobrancaVistoria : BaseServiceTest<ISolicitacaoService>
    {
        private ISolicitacaoQueries _querieSolicitacao;
       
        [TestInitialize]
        public void SolicitacaoReInspecaoInitialize()
        {
            AutenticarUsuario(1);
            _querieSolicitacao = _ioc.GetInstance<ISolicitacaoQueries>();
        }

        [TestMethod]
        public void ListarSolicitacoes()
        {
            var lst = _querieSolicitacao.SolicitacoesCobrancaVistoria();

            Assert.IsNotNull(lst);
            Assert.IsTrue(lst.Count > 0);
        }

        [TestMethod]
        public void EnviarEmail()
        {
            var lst = _querieSolicitacao.SolicitacoesCobrancaVistoria();

            foreach (var item in lst)
            { 
                _service.EnviarEmailCobrancaVistoria(item, 1);
            }

            Assert.IsNotNull(lst);
            Assert.IsTrue(lst.Count > 0);
        }
    }
}
