using ImageSlideShow.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ImageSlideShow.Test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static List<string> ExtersionList = new List<string>() { ".jpg", ".png", ".bmp", ".gif", ".jpeg", ".jpe", ".ico" };
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Initialize(@"C:\Zoos\files\sharedocument");
        }

        public void Initialize(string path)
        {
            this.imgeslideshow.Clear(true);
            FileInfo[] fileInfos = new DirectoryInfo(path).GetFiles();
            List<string> fileDirModelList = new List<string>();
            int i = 0;
            int curIndex = 0;
            foreach (FileInfo file in fileInfos)
            {
                if ((file.Attributes & FileAttributes.System) != FileAttributes.System &&
                                (file.Attributes & FileAttributes.Hidden) != FileAttributes.Hidden)
                {
                    if (ExtersionList.Contains(file.Extension.ToLower()))
                    {

                        int width = 1920 * 2;// System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
                        int height = 1080 * 2;// System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;
                        MemoryStream ms = ImgManager.GetImagebyMemoryStream(file.FullName, width, height);


                        //MemoryStream ms = ImgCore.GetNewImageStream(file.FullName, width, height);
                        this.imgeslideshow.AddItem(ms, file.FullName, null, file.FullName);
                        i++;
                    }
                }
            }
            this.imgeslideshow.InitSlidShow(0);
        }

    }
}
