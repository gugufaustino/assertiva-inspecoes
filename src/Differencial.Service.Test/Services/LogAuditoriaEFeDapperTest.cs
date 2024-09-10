using Differencial.Service.Services;
using Differencial.Domain.Contracts.Entities;
using Differencial.Domain.Contracts.Services;
using Differencial.Service.Test.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using Differencial.Domain.Filters;
using Differencial.Domain.Entities;

namespace Differencial.Service.Services.Tests
{
    [TestClass]
    public class LogAuditoriaEFeDapperTest : BaseServiceTest<ILogAuditoriaService>
    {
        [TestMethod]
        public void ListarTodosEFTest()
        {
            var entidade = _service.ListarTodosEF();
            Assert.IsTrue(entidade.Count >= 0);
        }

        [TestMethod]
        public void ListarTodosDPTest()
        {
            var entidade = _service.ListarTodosDP();
            Assert.IsTrue(entidade.Count >= 0);
        }

        [TestMethod]
        public void ServicoIguaisEFeDapperTest()
        {
            var entidadeDP = _service.ListarTodosDP();
            var entidadeEF = _service.ListarTodosEF();
            Assert.AreEqual(entidadeDP.Count, entidadeEF.Count);
        }

        [TestMethod]
        public void ServicoListarLogEFeDapperTest()
        {
            var _serviceSol = _ioc.GetInstance<ISolicitacaoService>();

            var entidade = _serviceSol.Buscar(20000);

            var lstEntidadeLogs = new List<IEntity>();
            lstEntidadeLogs.Add(entidade);

            if (entidade.Endereco != null)
                lstEntidadeLogs.Add(entidade.Endereco);

            if (entidade.Foto.Any())
                lstEntidadeLogs.AddRange(entidade.Foto.ToList());

            if (entidade.Cobertura.Any())
                lstEntidadeLogs.AddRange(entidade.Cobertura.ToList());

            if (entidade.Agendamento.Any())
                lstEntidadeLogs.AddRange(entidade.Agendamento.ToList());

            if (entidade.LancamentoFinanceiro.Any())
                lstEntidadeLogs.AddRange(entidade.LancamentoFinanceiro.ToList());

            var lstEF = _service.Listar(lstEntidadeLogs).ToList();
            var lstDP = _service.ListarDapper(lstEntidadeLogs).ToList();

            Assert.AreEqual(lstEF.Count, lstDP.Count);

        }

        [TestMethod]
        [Timeout(3000)]
        [DataTestMethod]
        [DataRow(0)]
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(3)]
        [DataRow(10)]
        public void ListarPaginacaoTest(int pagina)
        {
            // arrange
            LogAuditoriaFilter LogPag = new LogAuditoriaFilter
            {
                Skip = pagina * 25,
                Take = 25, 
            };

            // act
            List<LogAuditoria> lstEf = _service.Listar(LogPag).ToList();
            List<LogAuditoria> lstDP = _service.ListarDP(LogPag).ToList();

            // assert 
            for (int i = 0; i < lstEf.Count; i++)
            {
                LogAuditoria a = lstEf[i];
                LogAuditoria b = lstDP[i];  
                Assert.IsTrue(a.Equals(b), string.Format("Comparação do elemento {0} da coleção. A.ID {1} == B.ID {2}", i.ToString(), a.Id.ToString(), b.Id.ToString()));
            }

            Assert.AreNotEqual(LogPag.TotalRecords, 0);
            Assert.AreEqual(lstEf.Count, LogPag.Take);
            CollectionAssert.AllItemsAreUnique(lstEf);
            
        }

        [TestMethod]
        [Timeout(3000)]
        [DataTestMethod]
        [DataRow(2, DisplayName = "UsuarioAplicacao")]
        public void ListarFiltroTest(int usuarioAplicacao)
        {
            // arrange
            LogAuditoriaFilter LogPag = new LogAuditoriaFilter
            {
                Skip = 0,
                Take = 25,
                UsuarioAplicacao = usuarioAplicacao
            };

            // act
            var lst = _service.Listar(LogPag).ToList();
            var lstMesmosFiltros = lst.Where(i => i.UsuarioAplicacao == LogPag.UsuarioAplicacao).ToList();

            // assert 
            CollectionAssert.AllItemsAreUnique(lst);
            CollectionAssert.AreEqual(lst, lstMesmosFiltros);

        }

        
    }
}
