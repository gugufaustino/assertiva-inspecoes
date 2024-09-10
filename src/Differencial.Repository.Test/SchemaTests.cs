using Microsoft.VisualStudio.TestTools.UnitTesting;
using Differencial.Repository.Context;
using System;
using System.Data.Entity;

namespace Differencial.Repository.Test
{
    [TestClass]
    public class SchemaTests
    {
        [TestMethod]
        public void Pode_gerar_esquema_bd()
        {
            Database.SetInitializer<DifferencialContext>(null);
            var contexto = new DifferencialContext("Test");
            string result = contexto.CreateDatabaseScript();
            Assert.IsNotNull(result);
            Console.Write(result);
        }
    }
}
