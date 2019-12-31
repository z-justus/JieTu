using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public class ImageConversion
    {
        public static string ImageToByte64String(Bitmap bitmap, ImageFormat format)
        {
            MemoryStream memoryStream = new MemoryStream();
            bitmap.Save(memoryStream, format);
            byte[] array = new byte[memoryStream.Length];
            memoryStream.Position = 0L;
            memoryStream.Read(array, 0, (int)memoryStream.Length);
            memoryStream.Close();
            return Convert.ToBase64String(array);
        }
    }
}
