using Differencial.Domain.Util.ExtensionMethods;
using System;
using System.Drawing;
using System.IO;
using static Differencial.Service.ImageResizer;

namespace Differencial.Service
{

    /// <summary>
    /// Código de Referencia em:
    /// https://www.dotnetperls.com/getthumbnailimage
    /// </summary>
    public static class ImageHelper
    {
        public static Stream MontarThumbnailImage(Stream arquivoStream)
        {
            // Load image.
            Image image = Image.FromStream(arquivoStream);

            // Compute thumbnail size.
            Size thumbnailSize = MontarNovoTamanho(image, 140);

            // Get thumbnail.
            Image thumbnail = image.GetThumbnailImage(thumbnailSize.Width, thumbnailSize.Height, null, IntPtr.Zero);
             
            // New thumbnail.
            MemoryStream imgThumb = new MemoryStream();

            thumbnail.Save(imgThumb, image.RawFormat);
            imgThumb.Seek(0, SeekOrigin.Begin);

            return imgThumb;
        }

        private static Size MontarNovoTamanho(Image original, int maxPixels)
        {
            // Width and height.
            int originalWidth = original.Width;
            int originalHeight = original.Height;

            // Compute best factor to scale entire image based on larger dimension.
            double factor;
            if (originalWidth > originalHeight)
            {
                factor = (double)maxPixels / originalWidth;
            }
            else
            {
                factor = (double)maxPixels / originalHeight;
            }

            // Return thumbnail size.
            return new Size((int)(originalWidth * factor), (int)(originalHeight * factor));
        }

        
        public static Stream ReduzirTamanho(Stream imageStream, int maxPixels)
        {
            var image = Image.FromStream(imageStream);

            Size newSize = MontarNovoTamanho(image, maxPixels);

            var newimageStream = new ImageResizer(imageStream.ToByteArray())
                                        .Resize(newSize.Width, newSize.Height, ImageEncoding.Jpg);
            
            return new MemoryStream(newimageStream);

        }
    }

    internal class ImageResizer
    {
        private byte[] vs;

        public ImageResizer(byte[] vs)
        {
            this.vs = vs;
        }

        internal byte[] Resize(int width, int height, ImageEncoding jpg)
        {
            throw new NotImplementedException();
        }

        public enum ImageEncoding
        {
            Jpg
        }


    }
}
