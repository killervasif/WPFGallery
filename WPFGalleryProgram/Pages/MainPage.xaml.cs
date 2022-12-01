using Microsoft.Win32;
using Ookii.Dialogs.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace WPFGalleryProgram.Pages
{
    public partial class MainPage : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        public List<ImageSource> ImageSources { get; set; }

        private int rows = 3;

        public int Rows
        {
            get { return rows; }
            set
            {
                rows = value;
                OnPropertyChanged();
            }
        }


        private int columns = 3;

        public int Columns
        {
            get { return rows; }
            set
            {
                rows = value;
                OnPropertyChanged();
            }
        }

        public MainPage()
        {
            InitializeComponent();

            DataContext = this;

            ImageSources = new();
        }


        private void lbx_DragOver(object sender, DragEventArgs e)
        {
            bool dropEnabled = true;
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] filenames =
                                 e.Data.GetData(DataFormats.FileDrop, true) as string[];

                foreach (string fileName in filenames)
                {
                    if (System.IO.Path.GetExtension(fileName).ToUpperInvariant() != ".JPG" && System.IO.Path.GetExtension(fileName).ToUpperInvariant() != ".JPEG" && System.IO.Path.GetExtension(fileName).ToUpperInvariant() != ".PNG")
                    {
                        dropEnabled = false;
                        break;
                    }
                }
            }
            else
            {
                dropEnabled = false;
            }

            if (!dropEnabled)
            {
                e.Effects = DragDropEffects.None;
                e.Handled = true;
            }
        }


        private void lbx_Drop(object sender, DragEventArgs e)
        {
            foreach (var filename in e.Data.GetData(DataFormats.FileDrop) as string[])
            {
                var image = new Image()
                {
                    Source = new BitmapImage(new Uri(filename)),
                    Width = 100,
                    Height = 100,
                    MinHeight = 70,
                    MinWidth = 70,
                    Stretch = Stretch.Uniform
                };

                lbx.Items.Add(image);

                ImageSources.Add(image.Source);

                if (lbx.Items.Count > (Rows * Columns))
                    ++Rows;

            }

            

        }


        private void lbx_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (lbx.SelectedItem is Image image)
            {
                PhotoPage photoPage = new(image.Source, ImageSources);

                NavigationService.Navigate(photoPage);
            }
        }

        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Filter = "Images|*.png;*.jpg;*.jpeg"
            };


            if (openFileDialog.ShowDialog() == true)
            {
                var image = new Image()
                {
                    Source = new BitmapImage(new Uri(openFileDialog.FileName)),
                    Width = 100,
                    Height = 100,
                    MinHeight = 70,
                    MinWidth = 70,
                    Stretch = Stretch.Uniform
                };

                lbx.Items.Add(image);

                ImageSources.Add(image.Source);
            }

        }


        private void OpenFolder_Click(object sender, RoutedEventArgs e)
        {
            VistaFolderBrowserDialog dialog = new();


            if (dialog.ShowDialog() == true)
            {
                var directoryInfo = new DirectoryInfo(dialog.SelectedPath);

                foreach (var file in directoryInfo.GetFiles())
                {
                    string fileName = file.Name;

                    if (Path.GetExtension(fileName).ToUpperInvariant() == ".JPG" || Path.GetExtension(fileName).ToUpperInvariant() == ".JPEG" || Path.GetExtension(fileName).ToUpperInvariant() == ".PNG")
                    {

                        var image = new Image()
                        {
                            Source = new BitmapImage(new Uri(file.FullName)),
                            Stretch = Stretch.Uniform
                        };

                        lbx.Items.Add(image);

                        ImageSources.Add(image.Source);

                    }
                }
            }

        }

        private void BtnSave_Click(object sender, RoutedEventArgs e) => MessageBox.Show("Will Be Updated Soon");

        private void BtnView_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem item)
            {
                Columns = item.Header switch
                {
                    "Medium Icons" => 7,
                    "Small Icons" => 10,
                    _ => 3
                };
            }
        }
    }
}
