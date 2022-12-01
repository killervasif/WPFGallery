using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;

namespace WPFGalleryProgram.Pages;

public partial class PhotoPage : Page, INotifyPropertyChanged
{

    bool isAutoPlayActice = false;

    private ImageSource _currentImageSource;

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? name = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    public ImageSource CurrentImageSource
    {
        get { return _currentImageSource; }
        set
        {
            _currentImageSource = value;
            OnPropertyChanged();
        }
    }


    public List<ImageSource> ImageSources { get; set; }


    public PhotoPage(ImageSource currentImageSource, List<ImageSource> imageSources)
    {
        InitializeComponent();
        DataContext = this;
        _currentImageSource = currentImageSource;
        ImageSources = imageSources;
    }


    private void Page_Loaded(object sender, RoutedEventArgs e)
    {
        DispatcherTimer dispatcherTimer = new DispatcherTimer();
        dispatcherTimer.Tick += DispatcherTimer_Tick; ;
        dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
        dispatcherTimer.Start();

    }

    private void DispatcherTimer_Tick(object? sender, EventArgs e)
    {
        if (isAutoPlayActice)
            BtnForward_Click(sender, new RoutedEventArgs());
    }

    private void BtnForward_Click(object sender, RoutedEventArgs e)
    {
        var index = ImageSources.IndexOf(CurrentImageSource);

        if (index + 1 < ImageSources.Count)
        {
            CurrentImageSource = ImageSources[index + 1];
            return;
        }

        CurrentImageSource = ImageSources[0];
    }

    private void BtnBackward_Click(object sender, RoutedEventArgs e)
    {
        var index = ImageSources.IndexOf(CurrentImageSource);

        if (index - 1 >= 0)
        {
            CurrentImageSource = ImageSources[index - 1];
            return;
        }

        CurrentImageSource = ImageSources[ImageSources.Count - 1];
    }

    private void BtnBack_Click(object sender, RoutedEventArgs e) => NavigationService.GoBack();

    private void BtnAutoPlay_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button button)
        {
            isAutoPlayActice = button.Name switch
            {
                "BtnPlay" => true,
                _ => false
            };
        }
    }
}
