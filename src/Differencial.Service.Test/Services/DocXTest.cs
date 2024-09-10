using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Differencial.Domain.Contracts.Services;
using Differencial.Domain.Contracts.Util;
using System.Collections.Generic;
using Differencial.Domain.Entities;
using SimpleInjector;
using Differencial.Domain.Util;

namespace Differencial.Service.Test.ServicesTests
{
    [TestClass]
    public class DocXTest
    {
        protected static IOC.IOC _ioc = new IOC.IOC(true);
        IDocXService _service;
        [TestInitialize]
        public void TestInitialize()
        {

            var _lifetimeScope = _ioc.Container.BeginLifetimeScope();
            IConfiguracaoAplicativo configApp = _ioc.GetInstance<IConfiguracaoAplicativo>();
            _service = new DocXService(configApp);

        }

        [TestMethod]
        public void TestMethod1()
        {
            var lstFoto = new List<ArquivoAnexo>() {
                new ArquivoAnexo
                {
                    IdSolicitacao = 1,
                    ArquivoNome = "teste1-4x3",
                    GuidArquivo = new Guid("3c6dd19f-f835-47b9-882f-e17e84a2cdb1"),
                },
                new ArquivoAnexo
                {
                    IdSolicitacao = 1,
                    ArquivoNome = "teste2-3x4",
                    GuidArquivo = new Guid("195e6f3b-e2a9-4980-b2b9-2da4997d11fe"),
                },

                  new ArquivoAnexo
                {
                    IdSolicitacao = 1,
                    ArquivoNome = "teste3-4x3",
                    GuidArquivo = new Guid("d2e8b41c-8c01-4eaa-9dab-b1eda1df15d6"),
                },

                new ArquivoAnexo
                {
                    IdSolicitacao = 1,
                    ArquivoNome = "teste4-4x3",
                    GuidArquivo = new Guid("7d46c23a-7e73-4b54-b89a-4cb7be2868d8"),
                }
            };
            _service.MontarQuadro(lstFoto, false);
        }

        [TestMethod]
        public void AspectRatio()
        {
            int Width = 1024; // 4 : 3
            int Height = 768; // 1944 ou 1456

            int nGCD = GetGreatestCommonDivisor(Height, Width);
            string str = string.Format("{0}:{1}", Width / nGCD, Height / nGCD);

            str = "";
        }
        public int GetGreatestCommonDivisor(int a, int b)

        {
            return b == 0 ? a : GetGreatestCommonDivisor(b, a % b);
        }

    }
}
