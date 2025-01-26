using Microsoft.VisualBasic.FileIO;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
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
                DirectoryFilesTextBox.Text = Path.GetDirectoryName(dialog.FileName);
            }
        }

        private void BrowseFilteredFolder_Click(object sender, RoutedEventArgs e)
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
                FilteredDirectoryTextBox.Text = Path.GetDirectoryName(dialog.FileName);
            }
        }
        private List<string> GetSelectedFormats()
        {
            var selectedFormats = new List<string>();

            foreach (TabItem tab in FileFormatsTabControl.Items)
            {
                if (tab.Content is Grid grid)
                {
                    foreach (var child in grid.Children)
                    {
                        if (child is WrapPanel wrapPanel)
                        {
                            foreach (var item in wrapPanel.Children)
                            {
                                if (item is CheckBox checkBox && checkBox.IsChecked == true)
                                {
                                    var formats = checkBox.Tag.ToString().Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                                    selectedFormats.AddRange(formats);
                                }
                            }
                        }
                    }
                }
            }
            if (!string.IsNullOrWhiteSpace(CustomFormatsTextBox.Text))
            {
                var customFormats = CustomFormatsTextBox.Text.Split(new[] { ',', ';', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                selectedFormats.AddRange(customFormats);
            }
            return selectedFormats;
        }

        private void FilterFiles_Click(object sender, RoutedEventArgs e)
        {
            string sourceDirectory = DirectoryFilesTextBox.Text;
            string destinationDirectory = null; 

            if (string.IsNullOrEmpty(sourceDirectory))
            {
                MessageBox.Show("Please specify the directory with the files and the directory to save them to.");
                return;
            }

            if (DeleteTheFilesCheckBox.IsChecked == true)
            {
                destinationDirectory = null;
            }
            else
            {
                destinationDirectory = Path.Combine(FilteredDirectoryTextBox.Text, FilteredFolderNameTextBox.Text);

                if (string.IsNullOrEmpty(destinationDirectory))
                {
                    MessageBox.Show("Please specify a directory for saving.");
                    return;
                }

                if (string.IsNullOrEmpty(FilteredFolderNameTextBox.Text))
                {
                    destinationDirectory = Path.Combine(FilteredDirectoryTextBox.Text, "FilteredFiles");
                }

                Directory.CreateDirectory(destinationDirectory); 
            }

            var selectedFormats = GetSelectedFormats();
            if (selectedFormats.Count == 0)
            {
                MessageBox.Show("Please select your file formats.");
                return;
            }

            if (!long.TryParse(MinFileSizeTextBox.Text, out long minSizeKB))
            {
                minSizeKB = 0;
            }

            if (!long.TryParse(MaxFileSizeTextBox.Text, out long maxSizeKB))
            {
                maxSizeKB = long.MaxValue;
            }

            DateTime? filterDateAfter = FilterDateAfterPicker.SelectedDate;
            DateTime? filterDateBefore = FilterDateBeforePicker.SelectedDate?.AddDays(1);

            try
            {
                var files = Directory.GetFiles(sourceDirectory)
                    .Where(file =>
                    {
                        bool formatMatches = selectedFormats.Any(format => file.EndsWith(format, StringComparison.OrdinalIgnoreCase));
                        long fileSizeKB = new FileInfo(file).Length / 1024;
                        bool sizeMatches = fileSizeKB >= minSizeKB && fileSizeKB <= maxSizeKB;

                        DateTime creationDate = File.GetCreationTime(file);
                        bool dateMatches = (!filterDateAfter.HasValue || creationDate >= filterDateAfter) &&
                                           (!filterDateBefore.HasValue || creationDate < filterDateBefore);

                        return formatMatches && sizeMatches && dateMatches;
                    });

                foreach (var file in files)
                {
                    string fileName = Path.GetFileName(file);

                    if (DeleteTheFilesCheckBox.IsChecked == true)
                    {
                        FileSystem.DeleteFile(file, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
                    }
                    else
                    {
                        string destinationFile = Path.Combine(destinationDirectory, fileName);
                        File.Move(file, destinationFile);
                    }
                }

                if (DeleteTheFilesCheckBox.IsChecked == true)
                {
                    MessageBox.Show("Files are successfully filtered and deleted to the recycle bin!");
                }
                else
                {
                    MessageBox.Show("Files are successfully filtered and moved!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
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