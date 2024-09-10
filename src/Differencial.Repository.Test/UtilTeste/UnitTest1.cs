using System;
using Differencial.Repository.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Differencial.Domain.Entities;
using System.Diagnostics;

namespace Differencial.Repository.Test.UtilTeste
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Debug.WriteLine("Filial.BlogId maps to: {0}", EFColumnName. GetColumnName(typeof(Filial), nameof(Filial.NomeFilial)));

        }
    }
}
