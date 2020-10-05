using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ImageSlideShow.Core
{
    public class ImgManager
    {
        public static MemoryStream GetImagebyMemoryStream(string imgPath, double maxWidth, double maxHeight)
        {
            Bitmap bitmap = null;
            MemoryStream outStream = null;
            try
            {
                BitmapImage bi = GetImagebyBitmapImage(imgPath, maxWidth, maxHeight);
                if (bi == null) return null;

                outStream = new MemoryStream();
                PngBitmapEncoder enc = new PngBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bi));
                enc.Save(outStream);
 
                if (bitmap != null)
                {
                    bitmap.Dispose();
                    bitmap = null;
                }
                return outStream;
            }
            catch
            {
                return null;
            }
     
        }

        public static BitmapImage GetImagebyBitmapImage(string imgPath, double maxWidth, double maxHeight)
        {
            if (File.Exists(imgPath))
            {
                try
                {
                    double newCx = 0, newCy = 0;
                    BitmapFrame bf = BitmapFrame.Create(new Uri(imgPath, UriKind.RelativeOrAbsolute), BitmapCreateOptions.DelayCreation | BitmapCreateOptions.IgnoreImageCache, BitmapCacheOption.None);
                    newCx = bf.PixelWidth;
                    newCy = bf.PixelHeight;

                    double newWidth = 0d;
                    double newHeight = 0d;
                    double perWidth = maxWidth / newCx;
                    double perHeight = maxHeight / newCy;
                    if (perWidth > perHeight)
                    {
                        newWidth = perHeight * newCx;
                        newHeight = maxHeight;
                    }
                    else
                    {
                        newWidth = maxWidth;
                        newHeight = perWidth * newCy;
                    }

                    newWidth = newWidth >= bf.PixelWidth ? bf.PixelWidth : newWidth;
                    newHeight = newHeight >= bf.PixelHeight ? bf.PixelHeight : newHeight;



                    BitmapImage bi = new BitmapImage();
                    bi.BeginInit();
                    bi.CreateOptions = BitmapCreateOptions.None;
                    bi.DecodePixelWidth = (int)newWidth;
                    bi.DecodePixelHeight = (int)newHeight;
                    bi.CacheOption = BitmapCacheOption.OnLoad;
                    bi.UriSource = new Uri(imgPath, UriKind.RelativeOrAbsolute);
                    bi.EndInit();
                    bf.Freeze();



                    return bi;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }

            return null;
        }
    }
}
