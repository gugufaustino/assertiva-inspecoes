using Differencial.Domain.Contracts.Services;
using Differencial.Service.Test.Services;
using Differencial.Service.Test.ServRefConsultaFinanceiro;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Differencial.Service.Test.ServicesTests
{
    [TestClass]
    public class ServiceTest : BaseServiceTest<ITipoInspecaoService>
    {  
        //[TestInitialize]
        //public void TestInitialize()
        //{
     
        //} 

        [TestMethod]
        public void TesteBuscar()
        {
            var entidade = _service.Buscar(1);
            Assert.IsTrue(entidade.NomeTipoInspecao == "Empresarial");
        }


        public static string[] triangleOrNot(int[] a, int[] b, int[] c)
        {

            var retornoTriangulo = new string[a.Length];
            for (int i = 0; i < a.Length; i++)
            {
                var ladoAB = a[i] + b[i];
                var ladoBC = b[i] + c[1];
                var ladoAC = a[i] + c[1];

                if (ladoAB >= ladoBC + ladoAC || ladoBC >= ladoAB + ladoAC || ladoAC >= ladoBC + ladoAB)
                    retornoTriangulo[i] = "NO";
                else
                    retornoTriangulo[i] = "YES";

            }

            return retornoTriangulo;
        }

        [TestMethod]
        public void TesteFinanceiro()
        {
            ConsultaFinanceiroClient cliente = new ConsultaFinanceiroClient();

            var lst = cliente.ListarMovimento(9, 2018, 1);

            cliente.Close();
        }
    }
}
