using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.IO;
using Differencial.Domain.Util.ExtensionMethods;

namespace Differencial.Service.Test.ServicesTests
{
    [TestClass]
    public class ImagemTest
    {

        [TestInitialize]
        public void TestInitialize()
        {


        }



        [TestMethod]
        [DataTestMethod]
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(3)]

        public void ReduzirTamanhoTest(int numFoto)
        {
            //A
            var imagePath = AppDomain.CurrentDomain.BaseDirectory + @"\..\..\Resources\";
            byte[] szOriginal, szNewLow;

            var imagemStream = File.OpenRead(imagePath + "teste-img (" + numFoto + ").jpg");
            //A
            var imagemStreamLow = ImageHelper.ReduzirTamanho(imagemStream, 1024);
            //A
            szOriginal = imagemStream.ToByteArray();
            szNewLow = imagemStreamLow.ToByteArray();

            Assert.IsTrue(szOriginal.Length > szNewLow.Length);

            File.WriteAllBytes(imagePath + "teste-img (" + numFoto + ")-processado-tamanho.jpg", szNewLow);
        }
        [TestMethod]
        [DataTestMethod]
        [DataRow(4)]

        public void ReduzirTamanhoPNGTest(int numFoto)
        {
            //A
            var imagePath = AppDomain.CurrentDomain.BaseDirectory + @"\..\..\Resources\";
            byte[] szOriginal, szNew;

            var imagemStream = File.OpenRead(imagePath + "teste-img (" + numFoto + ").png");
            //A
            var imagemStreamLow = ImageHelper.ReduzirTamanho(imagemStream, 1024);
            //A
            szOriginal = imagemStream.ToByteArray();
            szNew = imagemStreamLow.ToByteArray();

            Assert.IsTrue(szOriginal.Length > szNew.Length);

            File.WriteAllBytes(imagePath + "teste-img (" + numFoto + ")-processado-tamanho.png", szNew);
        }

        [TestMethod]
        [DataTestMethod]
        [DataRow(1)]
        [DataRow(2)]
        public void GerarTumbnailTest(int numFoto)
        {
            //A
            var imagePath = AppDomain.CurrentDomain.BaseDirectory + @"\..\..\Resources\";
            byte[] szOriginal, szNew;

            var imagemStream = File.OpenRead(imagePath + "teste-img (" + numFoto + ").png");
            //A
            var imgThumb = ImageHelper.MontarThumbnailImage(imagemStream);
            //A
            szOriginal = imagemStream.ToByteArray();
            szNew = imgThumb.ToByteArray();

            Assert.IsTrue(szOriginal.Length > szNew.Length);

            File.WriteAllBytes(imagePath + "teste-img (" + numFoto + ")-processado-thumbail.png", szNew);
        }
    }
}
