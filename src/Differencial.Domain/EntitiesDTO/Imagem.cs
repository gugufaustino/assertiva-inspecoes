using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace Differencial.Domain.EntitiesDTO
{
    public class Imagem
    {
        Image image;


        public Imagem(Stream stream)
        {
            image = Image.FromStream(stream);


        }

        public string MimeType
        {
            get
            {
                return ImageCodecInfo.GetImageEncoders().FirstOrDefault(x => x.FormatID == image.RawFormat.Guid).MimeType;

            }
        }

        public SizeF Dimensao
        {
            get
            {
                return image.PhysicalDimension;
            }
        }
    }
}
