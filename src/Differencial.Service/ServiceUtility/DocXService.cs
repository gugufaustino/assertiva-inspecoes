using Differencial.Domain.Contracts.Services;
using Differencial.Domain.Contracts.Util;
using Differencial.Domain.Entities;
using System;
using System.Collections.Generic;


namespace Differencial.Service
{
    public class DocXService : IDocXService
    {
        const int _height = 225;
        const int _width = 300;
        IConfiguracaoAplicativo _configApp;

        public DocXService(IConfiguracaoAplicativo configuracaoAplicativo)
        {
            _configApp = configuracaoAplicativo;
        }
        public string MontarQuadro(List<ArquivoAnexo> lstFotos, bool bNumSeqLegenda)
        {
            throw new NotImplementedException();

            //if (lstFotos.Count == 0)
            //    return null;

            //string caminhoArquivoQuado = _configApp.RepositorioSolicitacao + "//" + lstFotos.First().IdSolicitacao + ".docx";
            //string caminhoarquivoFoto = _configApp.RepositorioSolicitacao + "//" + lstFotos.First().IdSolicitacao + "//{0}";

            //int QtdFotos = lstFotos.Count;

            //using (DocX document = DocX.Create(caminhoArquivoQuado))
            //{
            //    int QtdLinhasTb = QtdFotos;
            //    if (QtdLinhasTb % 2 == 1)
            //    {
            //        QtdLinhasTb += 1;
            //    }
            //    Table t = document.AddTable(QtdLinhasTb, 2);
            //    t.Alignment = Alignment.center;
            //    t.Design = TableDesign.TableGrid;

            //    for (int row = 0; row < QtdFotos; row += 2)
            //    {
            //        var arquivoAnexoA = lstFotos[row];
            //        Picture p1 = CreatePicture(document, string.Format(caminhoarquivoFoto, arquivoAnexoA.GuidArquivo), arquivoAnexoA, bNumSeqLegenda);

            //        //Coluna 1
            //        Cell cellPicC1 = t.Rows[row].Cells[0]; 
            //        cellPicC1.MarginLeft = 0;
            //        cellPicC1.MarginRight = 0;
            //        cellPicC1.VerticalAlignment = VerticalAlignment.Center;
            //        cellPicC1.Paragraphs.First().Alignment = Alignment.center;
            //        cellPicC1.Paragraphs.First().AppendPicture(p1);

            //        Cell cellTitleC1 = t.Rows[row + 1].Cells[0];
            //        cellTitleC1.MarginLeft = 0;
            //        cellTitleC1.MarginRight = 0;
            //        cellTitleC1.Paragraphs.First().Alignment = Alignment.center;
            //        cellTitleC1.Paragraphs.First().Append(p1.Name);
                    

            //        // Coluna 2
            //        if (row + 2 != QtdLinhasTb || QtdFotos % 2 == 0)
            //        {
            //            var arquivoAnexoB = lstFotos[row + 1];
            //            Picture p2 = CreatePicture(document, string.Format(caminhoarquivoFoto, arquivoAnexoB.GuidArquivo), arquivoAnexoB, bNumSeqLegenda);

            //            Cell cellRowPicC2 = t.Rows[row].Cells[1];
            //            cellRowPicC2.MarginLeft = 0;
            //            cellRowPicC2.MarginRight = 0;
            //            cellRowPicC2.VerticalAlignment = VerticalAlignment.Center;
            //            cellRowPicC2.Paragraphs.First().Alignment = Alignment.center;
            //            cellRowPicC2.Paragraphs.First().AppendPicture(p2);

            //            Cell cellTitleC2 = t.Rows[row + 1].Cells[1];
            //            cellTitleC2.MarginLeft = 0;
            //            cellTitleC2.MarginRight = 0;
            //            cellTitleC2.Paragraphs.First().Alignment = Alignment.center;
            //            cellTitleC2.Paragraphs.First().Append(p2.Name);
            //        }
            //    }

            //    document.InsertTable(t);
            //    // Save this document as Output.docx.
            //    document.Save();
            //}

            //return caminhoArquivoQuado;
        }

   

        //private Picture CreatePicture(DocX document, string caminhoarquivo, ArquivoAnexo arquivoAnexo, bool bNumSeqLegenda = false)
        //{
        //    Xceed.Words.NET.Image image = document.AddImage(caminhoarquivo);
        //    Picture pic = image.CreatePicture();

        //    Orientation orientation = pic.Height > pic.Width ? Orientation.Portrait : Orientation.Landscape;

        //    if (orientation == Orientation.Portrait)
        //    {
        //        // se retrato então altura maior q largura 
        //        pic.Height = _width;
        //        pic.Width = _height;
        //    }
        //    else
        //    {
        //        pic.Height = _height;
        //        pic.Width = _width;
        //    }

        //    pic.Name = string.Empty;

        //    if (bNumSeqLegenda)
        //        pic.Name = (arquivoAnexo.LaudoFoto.QuadroFotosPosicao + 1).ToString() + " - ";
        //    pic.Name += arquivoAnexo.ArquivoNome;

        //    return pic;
        //}

        // Write the given string into this Image.
        //private static void CoolExample(Xceed.Words.NET.Image img, string str)
        //{


        //    // Write "Hello World" into this Image.
        //    Bitmap b = new Bitmap(img.GetStream(FileMode.Open, FileAccess.ReadWrite));

        //    /*
        //     * Get the Graphics object for this Bitmap.
        //     * The Graphics object provides functions for drawing.
        //     */
        //    Graphics g = Graphics.FromImage(b);

        //    // Draw the string "Hello World".
        //    g.DrawString
        //    (
        //        str,
        //        new System.Drawing.Font("Tahoma", 20),
        //        Brushes.Blue,
        //        new PointF(0, 0)
        //    );

        //    // Save this Bitmap back into the document using a Create\Write stream.
        //    b.Save(img.GetStream(FileMode.Create, FileAccess.Write), System.Drawing.Imaging.ImageFormat.Png);
        //}


    }
}
