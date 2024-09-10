using System;
using System.IO;
using TheArtOfDev.HtmlRenderer.PdfSharp;
using Microsoft.AspNetCore.Hosting;
using PdfSharpCore;

public class HtmlToPDFService
{
    private readonly IWebHostEnvironment webHostEnvironment;

    public HtmlToPDFService(IWebHostEnvironment webHostEnvironment  )
    {
        this.webHostEnvironment = webHostEnvironment;
    }
    public  void Criar(string html, string mappath)
    {
        byte[] res = null;
        MemoryStream msRetorno = new MemoryStream();
        using (MemoryStream ms = new MemoryStream())
        {
            PdfGenerateConfig config = new PdfGenerateConfig();
            //config.SetMargins(5);
            config.PageSize = PageSize.A4;

            var pdf = PdfGenerator.GeneratePdf(html, config);
            pdf.Save(ms);
            res = ms.ToArray();

            string path = "";
            string webRootPath = webHostEnvironment.WebRootPath;
            string contentRootPath = webHostEnvironment.ContentRootPath;
            path = Path.Combine(webRootPath, "CSS");
            //or path = Path.Combine(contentRootPath , "wwwroot" ,"CSS" );
            throw new NotImplementedException("corrigir pasta nesse local");
            //File.WriteAllBytes(path, res);
        }
    }
}
