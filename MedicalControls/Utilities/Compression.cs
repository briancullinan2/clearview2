using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace EPIC.MedicalControls.Utilities
{
    // Token: 0x0200004F RID: 79
    public static class Compression
    {
        // Token: 0x060002B5 RID: 693 RVA: 0x00016A30 File Offset: 0x00014C30
        public static Bitmap DecompressImage(byte[] image)
        {
            Bitmap result;
            using (MemoryStream memoryStream = new MemoryStream(image))
            {
                using (MemoryStream memoryStream2 = new MemoryStream())
                {
                    using (GZipStream gzipStream = new GZipStream(memoryStream, CompressionMode.Decompress, false))
                    {
                        gzipStream.CopyTo(memoryStream2);
                        result = new Bitmap(memoryStream2);
                    }
                }
            }
            return result;
        }

        // Token: 0x060002B6 RID: 694 RVA: 0x00016AC8 File Offset: 0x00014CC8
        public static byte[] CompressImage(Bitmap image)
        {
            byte[] result;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (MemoryStream memoryStream2 = new MemoryStream())
                {
                    image.Save(memoryStream2, ImageFormat.Bmp);
                    memoryStream2.Seek(0L, SeekOrigin.Begin);
                    using (GZipStream gzipStream = new GZipStream(memoryStream, CompressionMode.Compress))
                    {
                        memoryStream2.CopyTo(gzipStream);
                    }
                    byte[] array = memoryStream.ToArray();
                    result = ((array.Length == 0) ? null : array);
                }
            }
            return result;
        }

        [DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);


        // Token: 0x060002B7 RID: 695 RVA: 0x00016B88 File Offset: 0x00014D88
        public static BitmapSource GetImageSource(DataLayer.Entities.Image imageEntity)
        {
            BitmapSource result;
            using (Bitmap bitmap = Compression.DecompressImage(imageEntity.ImageData))
            {
                IntPtr hbitmap = bitmap.GetHbitmap();
                BitmapSource bitmapSource = Imaging.CreateBitmapSourceFromHBitmap(hbitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                DeleteObject(hbitmap);
                result = bitmapSource;
            }
            return result;
        }
    }
}
