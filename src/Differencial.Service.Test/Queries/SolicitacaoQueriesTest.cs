using Differencial.Domain.Queries;
using Differencial.Service.Test.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Differencial.Service.Test.Queries
{
    [TestClass]
    public class SolicitacaoQueriesTest : BaseServiceTest<ISolicitacaoQueries>
    {



        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void Listar()
        {

            var lst = _service.SolicitacoesCobrancaVistoria();
            //assert
            Assert.IsNotNull(lst);            
            Assert.IsTrue(lst.Count > 0);

        }

    }
}
