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
using System.Text.RegularExpressions;

namespace Android_Assets_Utility
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        String workspacePath;
        string userprofile = Environment.ExpandEnvironmentVariables("%USERPROFILE%");
        const int quality = 90;

        string resourcePath = "\\app\\src\\main\\res";

        string[] densityPath = {
            "\\mipmap-hdpi",
            "\\mipmap-xhdpi",
            "\\mipmap-xxhdpi",
            "\\mipmap-xxxhdpi",
            "\\mipmap-mhdpi",
            "\\mipmap-anydpi-v26"
        };

        int[] densityValue = { 72, 96, 144, 192, 48, 144 };


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
            var inputPath = txtBoxName2.Text;
            string iconName = "\\ic_launcher.png";
            for (int i = 0; i < densityPath.Length; i++) {
                string outputFolder = workspacePath + resourcePath + densityPath[i];
                string outputPath = outputFolder + iconName;
                bool exists = System.IO.Directory.Exists(outputFolder);
                if (!exists)
                    System.IO.Directory.CreateDirectory(outputFolder);
                int size = densityValue[i];
                CreateSingleLauncherIcon(size, size, outputPath, inputPath);
            }
            
        }

        private void Make_Round_Launcher_Icons(object sender, RoutedEventArgs e)
        {
            var inputPath = txtBoxName2b.Text;
            string iconName = "\\ic_launcher_round.png";
            for (int i = 0; i < densityPath.Length; i++)
            {
                string outputFolder = workspacePath + resourcePath + densityPath[i];
                string outputPath = outputFolder + iconName;
                bool exists = System.IO.Directory.Exists(outputFolder);
                if (!exists)
                    System.IO.Directory.CreateDirectory(outputFolder);
                int size = densityValue[i];
                CreateSingleLauncherIcon(size, size, outputPath, inputPath);
            }
        }

        private void CreateSingleLauncherIcon(int newwidth, int newheight, string outputPath, string inputPath) {
            Console.WriteLine(inputPath);
            var image = new Bitmap(System.Drawing.Image.FromFile(inputPath));
            var resized = new Bitmap(newwidth, newheight);
            using (var graphics = Graphics.FromImage(resized))
            {
                graphics.CompositingQuality = CompositingQuality.HighSpeed;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.DrawImage(image, 0, 0, newwidth, newheight);
                //var output = File.Open(outputPath, FileMode.Create);
                Console.WriteLine(outputPath);
                using (var output = File.OpenWrite(outputPath))
                {
                    var qualityParamId = System.Drawing.Imaging.Encoder.Quality;
                    var encoderParameters = new EncoderParameters(1);
                    encoderParameters.Param[0] = new EncoderParameter(qualityParamId, quality);
                    var pngcodec = ImageCodecInfo.GetImageDecoders().FirstOrDefault(codec => codec.FormatID == ImageFormat.Png.Guid);
                    resized.Save(output, pngcodec, encoderParameters);
                }
            }
        }

        private void Make_Multiple_Size_Images(object sender, RoutedEventArgs e)
        {
            //app/src/main/res/drawable
            //app/src/main/res/drawable-xhdpi
            
        }

        private void Support_Multiple_Size_Screen(object sender, RoutedEventArgs e)
        {
                   
            string inputFile = workspacePath + resourcePath + "\\values\\dimens.xml";
            string outputFolder = workspacePath + resourcePath + "\\values-sw600dp";
            string outFile = workspacePath + resourcePath + "\\values-sw600dp\\dimens.xml";
            bool exists = System.IO.Directory.Exists(outputFolder);
            if (!exists)
                System.IO.Directory.CreateDirectory(outputFolder);
            //string pat = @"(\w+)\s+(car)";
            //Regex r = new Regex(pat, RegexOptions.IgnoreCase);
            //Match m = r.Match(text);
            GenerateSingleDimensFile(outFile, inputFile, 1.4);

            outputFolder = workspacePath + resourcePath + "\\values-sw720dp";
            outFile = workspacePath + resourcePath + "\\values-sw720dp\\dimens.xml";
            exists = System.IO.Directory.Exists(outputFolder);
            if (!exists)
                System.IO.Directory.CreateDirectory(outputFolder);

            GenerateSingleDimensFile(outFile, inputFile, 1.6);

        }

        private void GenerateSingleDimensFile(string outFile, string inputFile, double scale) {
            string line;
            using (StreamWriter outputFile = new StreamWriter(outFile))
            {
                using (StreamReader sr = new StreamReader(inputFile))
                {
                    while ((line = sr.ReadLine()) != null)
                    {

                        int start = line.IndexOf("<dimen", 0);                     
                        if (start != -1)
                        {
                            Console.WriteLine(line);
                            start = line.IndexOf(">", start);
                            int end = line.IndexOf("</dimen>", start);
                            //Console.WriteLine(start);
                            //Console.WriteLine(end);
                            string rawDim = line.Substring(start + 1, end-start-1);
                            //Console.WriteLine(rawDim);
                            string unit = rawDim.Substring(rawDim.Length - 2, 2);
                            double value = double.Parse(rawDim.Substring(0, rawDim.Length - 2));
                            value = scale * value;
                            string newDim = ">" + value + unit + "<";
                            //Console.WriteLine(newDim);
                            line = line.Replace(">" + rawDim + "<", newDim);
                        }
                        outputFile.WriteLine(line);
                    }
                }
            }
        }

        private void Extract_Layout_Dimensions(object sender, RoutedEventArgs e)
        {
            //Format : filename__component__attribute__value
            //app/src/main/res/values/dimens.xml
            //app/src/main/res/layout/*.xml
            string inputFolder = workspacePath + resourcePath + "\\layout";
            string outputFolder = workspacePath + resourcePath + "\\layout";
            string outputFile = workspacePath + resourcePath + "\\values\\dimens.xml";
            if (Directory.Exists(inputFolder)) {
                string[] fileEntries = Directory.GetFiles(inputFolder);
                string line;
                string filename;
                string component = "";
                string attribute;
                string value;
                string dimenLine = "";
                using (StreamWriter outFile = new StreamWriter(outputFile))
                {
                    filename = fileEntries[0].Replace(".xml", "");
                    filename = filename.Replace(inputFolder, "");
                    filename = filename.Replace("\\", "");
                    for (int i = 0; i < fileEntries.Length; i++)
                    {
                        using (StreamReader sr = new StreamReader(fileEntries[i]))
                        {
                            line = sr.ReadLine();
                            outFile.WriteLine("<resources>");
                            while ((line = sr.ReadLine()) != null)
                            {
                                //Console.WriteLine(line.Trim());
                                if (line.Trim().Contains("</") || line.Trim().Contains("/>"))
                                {
                                    component = "";
                                }
                                if (line.Trim().Contains("android:id=\"@+id/"))
                                {
                                    String pattern = "(android:id=\"@[+]id[/])([a-zA-Z0-9_]*)";
                                    Regex r = new Regex(pattern, RegexOptions.IgnoreCase);
                                    Match m = r.Match(line.Trim());
                                    component = m.Groups[2].Value;
                                    Console.WriteLine(component);
                                }
                                if (!component.Equals(""))
                                {
                                    String pattern = "([a-zA-Z_]*):([a-zA-Z_]*)=\"([0-9]+)([a-zA-Z]*)";
                                    Regex r = new Regex(pattern, RegexOptions.IgnoreCase);
                                    Match m = r.Match(line.Trim());
                                    attribute = m.Groups[2].Value;
                                    value = m.Groups[3].Value + m.Groups[4];
                                    string dimenname = filename + "_" + component + "_" + attribute + "_" + value;
                                    if (!attribute.Equals("") && !value.Equals(""))
                                    {
                                        dimenLine = "<dimen name=\"" + dimenname + "\">" + value + "</dimen>";
                                        Console.WriteLine(dimenname);
                                        outFile.WriteLine(dimenLine);
                                    }
                                }
                            }
                            outFile.WriteLine("</resources>");
                        }
                    }
                    
                }
            }
        }

        private void Extract_Layout_Strings(object sender, RoutedEventArgs e)
        {
            //app/src/main/res/values/strings.xml
        }

    }
}
