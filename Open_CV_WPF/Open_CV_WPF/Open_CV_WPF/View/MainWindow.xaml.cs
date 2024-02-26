using System.Windows;
using Open_CV_WPF.Service;
using System.ComponentModel;
using Open_CV_WPF.ViewModel;


namespace Open_CV_WPF.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindowViewModel _mainWindowViewModel;
        public MainWindow(MainWindowViewModel cameraService)
        {
            _mainWindowViewModel = cameraService ;
            InitializeComponent();
        }

        private async void Window_Closed(object sender, CancelEventArgs e)
        {
             e.Cancel = true;

            if (await _mainWindowViewModel.CameraFirstViewModel.CloseService() && await _mainWindowViewModel.CameraSecondViewModel.CloseService()) // your Test method or the one below
            {
                Closing -= Window_Closed;
                Close();
            }
        }
    }
}