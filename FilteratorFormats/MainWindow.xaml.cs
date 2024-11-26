using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
namespace FilteratorFormats
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Browse_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog
            {
                CheckFileExists = false,
                CheckPathExists = true,
                FileName = "Folder Selection.",
                Filter = "Folders|\n",
                ValidateNames = false
            };

            bool? result = dialog.ShowDialog();

            if (result == true && !string.IsNullOrWhiteSpace(Path.GetDirectoryName(dialog.FileName)))
            {
                DirectoryTextBox.Text = Path.GetDirectoryName(dialog.FileName);
            }
        }
        private void FilterFiles_Click(object sender, RoutedEventArgs e)
        {
            string sourceDirectory = DirectoryTextBox.Text;

            if (string.IsNullOrEmpty(sourceDirectory) || !Directory.Exists(sourceDirectory))
            {
                MessageBox.Show("Please select a valid directory.");
                return;
            }

            var selectedExtensions = GetSelectedExtensions();

            if (!selectedExtensions.Any())
            {
                MessageBox.Show("Please select at least one file format.");
                return;
            }

            string filteredDirectory = Path.Combine(sourceDirectory, "FilteredFiles");

            if (!Directory.Exists(filteredDirectory))
            {
                Directory.CreateDirectory(filteredDirectory);
            }

            foreach (var file in Directory.GetFiles(sourceDirectory))
            {
                string extension = Path.GetExtension(file).ToLower();
                if (selectedExtensions.Contains(extension))
                {
                    string destFile = Path.Combine(filteredDirectory, Path.GetFileName(file));
                    File.Move(file, destFile); 
                }
            }

            MessageBox.Show("Files have been filtered and moved.");
        }
        private List<string> GetSelectedExtensions()
        {
            var extensions = new List<string>();

            // Image and grapgics formats
            if (JpgCheckBox.IsChecked == true) extensions.AddRange(new[] { ".jpg", ".jpeg" });
            if (PngCheckBox.IsChecked == true) extensions.Add(".png");
            if (GifCheckBox.IsChecked == true) extensions.Add(".gif");
            if (BmpCheckBox.IsChecked == true) extensions.Add(".bmp");
            if (SvgCheckBox.IsChecked == true) extensions.Add(".svg");
            if (IcoCheckBox.IsChecked == true) extensions.Add(".ico");
            if (PSDCheckBox.IsChecked == true) extensions.Add(".ico");
            if (AICheckBox.IsChecked == true) extensions.Add(".ico");
            if (FigCheckBox.IsChecked == true) extensions.Add(".ico");
            if (SketchCheckBox.IsChecked == true) extensions.Add(".ico");

            // Video formats
            if (Mp4CheckBox.IsChecked == true) extensions.Add(".mp4");
            if (AviCheckBox.IsChecked == true) extensions.Add(".avi");
            if (MkvCheckBox.IsChecked == true) extensions.Add(".mkv");
            if (MovCheckBox.IsChecked == true) extensions.Add(".mov");
            if (MpegCheckBox.IsChecked == true) extensions.Add(".mpeg");
            if (FlvCheckBox.IsChecked == true) extensions.Add(".flv");

            // Audio formats
            if (Mp3CheckBox.IsChecked == true) extensions.Add(".mp3");
            if (WavCheckBox.IsChecked == true) extensions.Add(".wav");
            if (FlacCheckBox.IsChecked == true) extensions.Add(".flac");
            if (AacCheckBox.IsChecked == true) extensions.Add(".aac");
            if (OggCheckBox.IsChecked == true) extensions.Add(".ogg");
            if (WmaCheckBox.IsChecked == true) extensions.Add(".wma");

            // Archive formats
            if (ZipCheckBox.IsChecked == true) extensions.Add(".zip");
            if (RarCheckBox.IsChecked == true) extensions.Add(".rar");
            if (TarCheckBox.IsChecked == true) extensions.Add(".tar");
            if (SevenZCheckBox.IsChecked == true) extensions.Add(".7z");
            if (IsoCheckBox.IsChecked == true) extensions.Add(".iso");

            // Text and Document formats
            if (TxtCheckBox.IsChecked == true) extensions.Add(".txt");
            if (DocxCheckBox.IsChecked == true) extensions.AddRange(new[] { ".doc", ".docx" });
            if (OdtCheckBox.IsChecked == true) extensions.Add(".odt");
            if (OdsCheckBox.IsChecked == true) extensions.Add(".ods");
            if (PDFCheckBox.IsChecked == true) extensions.Add(".pdf");
            if (XlsxCheckBox.IsChecked == true) extensions.AddRange(new[] { ".xls", ".xlsx" });
            if (PptxCheckBox.IsChecked == true) extensions.AddRange(new[] { ".ppt", ".pptx" });
            if (RftCheckBox.IsChecked == true) extensions.Add(".rtf");
            if (CsvCheckBox.IsChecked == true) extensions.Add(".csv");

            // System formats
            if (ExeCheckBox.IsChecked == true) extensions.Add(".exe");
            if (DllCheckBox.IsChecked == true) extensions.Add(".dll");
            if (SysCheckBox.IsChecked == true) extensions.Add(".sys");
            if (BatCheckBox.IsChecked == true) extensions.Add(".bat");
            if (CfgCheckBox.IsChecked == true) extensions.Add(".cfg");
            if (LogCheckBox.IsChecked == true) extensions.Add(".log");
            if (BinCheckBox.IsChecked == true) extensions.Add(".bin");

            // Code and Script Formats
            if (CCheckBox.IsChecked == true) extensions.Add(".c");
            if (CppCheckBox.IsChecked == true) extensions.Add(".cpp");
            if (CsCheckBox.IsChecked == true) extensions.Add(".cs");
            if (JavaCheckBox.IsChecked == true) extensions.Add(".java");
            if (PyCheckBox.IsChecked == true) extensions.Add(".py");
            if (GoCheckBox.IsChecked == true) extensions.Add(".go");
            if (RsCheckBox.IsChecked == true) extensions.Add(".rs");
            if (RbCheckBox.IsChecked == true) extensions.Add(".rb");

            // Web Data formats
            if (HtmlCheckBox.IsChecked == true) extensions.Add(".html");
            if (CssCheckBox.IsChecked == true) extensions.Add(".css");
            if (JsCheckBox.IsChecked == true) extensions.Add(".js");
            if (JsonCheckBox.IsChecked == true) extensions.Add(".json");
            if (XmlCheckBox.IsChecked == true) extensions.Add(".xml");
            if (PhpCheckBox.IsChecked == true) extensions.Add(".php");

            // File formats of 3D modeling
            if (ObjCheckBox.IsChecked == true) extensions.Add(".obj");
            if (BlendCheckBox.IsChecked == true) extensions.Add(".blend");
            if (StlCheckBox.IsChecked == true) extensions.Add(".stl");
            if (FbxCheckBox.IsChecked == true) extensions.Add(".fbx");
            if (MaxCheckBox.IsChecked == true) extensions.Add(".max");
            if (ThreeDSCheckBox.IsChecked == true) extensions.Add(".3ds");
            if (STEPCheckBox.IsChecked == true) extensions.Add(".STEP");

            // Font Formats
            if (TtfCheckBox.IsChecked == true) extensions.Add(".ttf");
            if (OftCheckBox.IsChecked == true) extensions.Add(".oft");
            if (WoffCheckBox.IsChecked == true) extensions.AddRange(new[] { ".woff", ".woff2" });
            if (EotCheckBox.IsChecked == true) extensions.Add(".eot");
            var customFormats = CustomFormatsTextBox.Text
               .Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries)
               .Select(f => f.Trim().ToLower());

            foreach (var format in customFormats)
            {
                if (!string.IsNullOrWhiteSpace(format) && !extensions.Contains(format))
                {
                    extensions.Add(format.StartsWith(".") ? format : "." + format);
                }
            }
            return extensions;
        }

        private void MinimiseAppButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void CloseAppButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void MainWindow_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }
    }
}