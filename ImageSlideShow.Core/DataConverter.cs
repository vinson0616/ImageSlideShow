using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;


namespace ImageSlideShow.Core
{
    public class DataConverter
    {
        #region Color

        public static Color ConvertToColor(string StrValue)
        {
            string[] Values = StrValue.Split(',');
            if (Values.Length == 4)
            {
                return Color.FromArgb(byte.Parse(Values[0].Trim()), byte.Parse(Values[1].Trim()), byte.Parse(Values[2].Trim()), byte.Parse(Values[3].Trim()));
            }
            else
            {
                return Color.FromArgb(0, 0, 0, 0);
            }
        }

        #endregion

        #region ImageSource

        public static ImageSource ConvertBitmapToSource(System.Drawing.Bitmap Img)
        {
            return Imaging.CreateBitmapSourceFromHBitmap(Img.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
        }

        public static ImageSource GetImageSourceByFullName(string FullName)
        {
            FileStream fs = new FileStream(FullName, FileMode.Open, FileAccess.Read);
            ImageSource IS = GetImgSourceByStream(fs);
            fs.Close();
            fs.Dispose();

            return IS;
        }

        public static ImageSource GetImageSourceByFullName(string FullName, ImageFormat IF)
        {
            FileStream fs = new FileStream(FullName, FileMode.Open, FileAccess.Read);
            System.Drawing.Image ImageData = System.Drawing.Image.FromStream(fs, true, false);
            MemoryStream ms = new MemoryStream();
            ImageData.Save(ms, IF);
            ms.Seek(0, SeekOrigin.Begin);

            ImageData.Dispose();
            fs.Close();
            fs.Dispose();

            return GetImgSourceByStream(ms);
        }

        public static System.Drawing.Bitmap GetBitmapByFullName(string FullName)
        {
            FileStream fs = new FileStream(FullName, FileMode.Open, FileAccess.Read);
            System.Drawing.Bitmap BT = new System.Drawing.Bitmap(fs);
            fs.Close();
            fs.Dispose();

            return BT;
        }

        public static System.Drawing.Bitmap GetBitmapByFullName(string FullName, ImageFormat IF)
        {
            FileStream fs = new FileStream(FullName, FileMode.Open, FileAccess.Read);
            System.Drawing.Image ImageData = System.Drawing.Image.FromStream(fs, true, false);
            MemoryStream ms = new MemoryStream();
            ImageData.Save(ms, IF);
            ms.Seek(0, SeekOrigin.Begin);

            ImageData.Dispose();
            fs.Close();
            fs.Dispose();

            return new System.Drawing.Bitmap(ms);
        }

        public static void GetImgFitSize(double ActualWidth, double ActualHeight, double FitWidth, double FitHeight, ref double NewWidth, ref double NewHeight)
        {
            double perWidth = FitWidth / ActualWidth;
            double perHeight = FitHeight / ActualHeight;
            if (perWidth > perHeight)
            {
                NewWidth = perHeight * ActualWidth;
                NewHeight = FitHeight;
            }
            else
            {
                NewWidth = FitWidth;
                NewHeight = perWidth * ActualHeight;
            }
        }

        public static ImageSource GetImageSourceThumb(string FullName, int Width, int Height, ImageFormat IF)
        {
            MemoryStream ms = GetStreamThumb(FullName, Width, Height, IF);

            return GetImgSourceByStream(ms);
        }

        public static MemoryStream GetStreamThumb(string FullName, int Width, int Height, ImageFormat IF)
        {
            System.Drawing.Bitmap NewBmpImage = GetBitMapThumb(FullName, Width, Height);

            MemoryStream ms = new MemoryStream();
            NewBmpImage.Save(ms, IF);
            ms.Seek(0, SeekOrigin.Begin);

            NewBmpImage.Dispose();

            return ms;
        }

        public static System.Drawing.Bitmap GetBitMapThumb(Stream stream, int Width, int Height)
        {
            System.Drawing.Image ImageData = System.Drawing.Image.FromStream(stream, true, false);

            double NewWidth = 0;
            double NewHeight = 0;
            GetImgFitSize((double)ImageData.Width, (double)ImageData.Height, (double)Width, (double)Height, ref NewWidth, ref NewHeight);

            System.Drawing.Bitmap NewBmpImage = new System.Drawing.Bitmap((int)NewWidth, (int)NewHeight);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(NewBmpImage);
            g.Clear(System.Drawing.Color.Transparent);
            g.DrawImage(ImageData, 0, 0, (int)NewWidth, (int)NewHeight);

            ImageData.Dispose();
            g.Dispose();
            stream.Close();
            stream.Dispose();

            return NewBmpImage;
        }


        public static System.Drawing.Bitmap GetBitMapThumb(string FullName, int Width, int Height)
        {
            FileStream fs = new FileStream(FullName, FileMode.Open, FileAccess.Read);
            System.Drawing.Image ImageData = System.Drawing.Image.FromStream(fs, true, false);

            double NewWidth = 0;
            double NewHeight = 0;
            GetImgFitSize((double)ImageData.Width, (double)ImageData.Height, (double)Width, (double)Height, ref NewWidth, ref NewHeight);

            System.Drawing.Bitmap NewBmpImage = new System.Drawing.Bitmap((int)NewWidth, (int)NewHeight);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(NewBmpImage);
            g.Clear(System.Drawing.Color.Transparent);
            g.DrawImage(ImageData, 0, 0, (int)NewWidth, (int)NewHeight);

            ImageData.Dispose();
            g.Dispose();
            fs.Close();
            fs.Dispose();

            return NewBmpImage;
        }

        public static ImageSource GetImgSourceByStream(Stream ImgStream)
        {
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.CacheOption = BitmapCacheOption.OnLoad;
            bi.StreamSource = ImgStream;
            bi.EndInit();

            return bi;
        }

        public static ImageSource GetImgSourceByRelative(string FileName)
        {
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.UriSource = new Uri(FileName, UriKind.Relative);
            bi.EndInit();

            return bi;
        }

        public static ImageSource GetImgSourceByAbsolute(string FullName)
        {
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.UriSource = new Uri(FullName, UriKind.Absolute);
            bi.EndInit();

            return bi;
        }

        public static ImageSource GetImgSourceByFileName(string FileName)
        {
            FileName = FileName.Trim('/');
            FileName = FileName.Trim('\\');
            FileName = @"pack://siteOfOrigin:,,,/" + FileName;

            return DataConverter.GetImgSourceByAbsolute(FileName);
        }

        public static ImageSource GetImgSourceByFileName(string FileName, string AssemblyName)
        {
            FileName = FileName.Trim('/');
            FileName = FileName.Trim('\\');
            FileName = "pack://application:,,,/" + AssemblyName + ";Component/" + FileName;

            return DataConverter.GetImgSourceByAbsolute(FileName);
        }

        #endregion

        #region Clone

        public static void CloneObjectPropertyValues(object OriginObj, object TargetObj, bool TargetRule)
        {
            if (OriginObj != null && TargetObj != null)
            {
                PropertyInfo[] XmlPerObjList = null;
                if (TargetRule)
                {
                    XmlPerObjList = TargetObj.GetType().GetProperties();
                }
                else
                {
                    XmlPerObjList = OriginObj.GetType().GetProperties();
                }

                foreach (PropertyInfo XmlPerObj in XmlPerObjList)
                {
                    PropertyInfo PerObj = null;
                    if (TargetRule)
                    {
                        PerObj = OriginObj.GetType().GetProperty(XmlPerObj.Name);
                    }
                    else
                    {
                        PerObj = TargetObj.GetType().GetProperty(XmlPerObj.Name);
                    }

                    if (PerObj != null)
                    {
                        object value = PerObj.GetValue(OriginObj, null);
                        XmlPerObj.SetValue(TargetObj, value, null);
                    }
                }
            }
        }

        public object CloneObject(object Obj)
        {
            BinaryFormatter Formatter = new BinaryFormatter(null, new StreamingContext(StreamingContextStates.Clone));
            MemoryStream stream = new MemoryStream();
            Formatter.Serialize(stream, Obj);
            stream.Position = 0;
            object clonedObj = Formatter.Deserialize(stream);
            stream.Close();
            return clonedObj;
        }

        /// <summary>
        /// Deep clone the objects.
        /// </summary>
        /// <typeparam name="T">The type of cloning.</typeparam>
        /// <param name="source">source to cloning</param>
        /// <returns>Deep clone results.</returns>
        public static T DeepClone<T>(T source) where T : class
        {
            using (MemoryStream ms = new MemoryStream())
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(ms, source);
                ms.Position = 0;
                return (T)serializer.Deserialize(ms);
            }
        }

        /// <summary>
        /// Clone value by same property.
        /// </summary>
        /// <typeparam name="TResult">The result type.</typeparam>
        /// <typeparam name="TSource">The source type.</typeparam>
        /// <param name="source">source</param>
        /// <returns>The clone results.</returns>
        public static TResult CloneValues<TSource, TResult>(TSource source)
            where TSource : class
            where TResult : class
        {
            using (MemoryStream ms = new MemoryStream())
            {
                XmlSerializer serializer = new XmlSerializer(typeof(TSource));
                serializer.Serialize(ms, source);
                ms.Position = 0;
                serializer = new XmlSerializer(typeof(TResult));
                return (TResult)serializer.Deserialize(ms);
            }
        }

        #endregion
    }
}
