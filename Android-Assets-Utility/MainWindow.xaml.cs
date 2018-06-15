using System;
using System.Collections.Generic;
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
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace Android_Assets_Utility
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        String workspacePath;


        public MainWindow()
        {
            InitializeComponent();
        }

        private void Set_Project_Folder(object sender, RoutedEventArgs e)
        {
            workspacePath = txtBoxName1.Text;
        }

        private void Make_Launcher_Icons(object sender, RoutedEventArgs e)
        {
            //app/src/main/res/mipmap-hdpi/ic_launcher.png
            //app/src/main/res/mipmap-hdpi/ic_launcher_round.png
            //app/src/main/res/mipmap-xhdpi/ic_launcher.png
            //app/src/main/res/mipmap-xhdpi/ic_launcher_round.png
            //app/src/main/res/mipmap-xxhdpi/ic_launcher.png
            //app/src/main/res/mipmap-xxhdpi/ic_launcher_round.png
            //app/src/main/res/mipmap-xxxhdpi/ic_launcher_round.png
            //app/src/main/res/mipmap-xxxhdpi/ic_launcher.png
            //app/src/main/res/mipmap-mdpi/ic_launcher_round.png
            //app/src/main/res/mipmap-mdpi/ic_launcher.png
            //app/src/main/res/mipmap-anydpi-v26/ic_launcher_round.png
            //app/src/main/res/mipmap-anydpi-v26/ic_launcher.png
            var inputPath = txtBoxName2.Text;
            var image = new Bitmap(System.Drawing.Image.FromFile(inputPath));
            var resized = new Bitmap(width, height);
            var graphics = Graphics.FromImage(resized);
            graphics.CompositingQuality = CompositingQuality.HighSpeed;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.CompositingMode = CompositingMode.SourceCopy;
            graphics.DrawImage(image, 0, 0, width, height);
            var output = File.Open(OutputPath(path, outputDirectory, SystemDrawing), FileMode.Create);
            var qualityParamId = Encoder.Quality;
            var encoderParameters = new EncoderParameters(1);
            encoderParameters.Param[0] = new EncoderParameter(qualityParamId, quality);
            var codec = ImageCodecInfo.GetImageDecoders().FirstOrDefault(codec => codec.FormatID == ImageFormat.Png.Guid);
            resized.Save(output, codec, encoderParameters);
        }

        private void Make_Multiple_Size_Images(object sender, RoutedEventArgs e)
        {
            //app/src/main/res/drawable
            //app/src/main/res/drawable-xhdpi
        }

        private void Support_Multiple_Size_Screen(object sender, RoutedEventArgs e)
        {
            //app/src/main/res/values/dimens.xml
            //app/src/main/res/values-sw600dp/dimens.xml
            //app/src/main/res/values-sw720dp/dimens.xml
        }

        private void Extract_Layout_Dimensions(object sender, RoutedEventArgs e)
        {
            //app/src/main/res/values/dimens.xml
            //app/src/main/res/layout/*.xml
        }

        private void Extract_Layout_Strings(object sender, RoutedEventArgs e)
        {
            //app/src/main/res/values/strings.xml
        }

    }
}
