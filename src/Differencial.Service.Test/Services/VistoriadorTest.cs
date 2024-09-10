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
    public class VistoriadorTest : BaseServiceTest<IVistoriadorService>
    {
        [TestMethod]
        public void ListarTodosEFTest()
        {
            var entidade = _service.ListarOperadorDistancia((double)-30.008597, (double)-51.191220, 2, 3);

            Assert.IsTrue(entidade.Count >= 0);
            CollectionAssert.AllItemsAreUnique(entidade.Select(i=> new { Id = i.Id, NomNomeOperadore = i.NomeOperador, DistanciaRaio = i.DistanciaRaio }).ToList());
        } 
    }
}
