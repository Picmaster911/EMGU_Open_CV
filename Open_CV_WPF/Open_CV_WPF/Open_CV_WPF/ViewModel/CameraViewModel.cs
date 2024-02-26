using Microsoft.Win32;
using Open_CV_WPF.Infrastructure.Commands;
using Open_CV_WPF.Service;
using System.Windows.Media;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;

namespace Open_CV_WPF.ViewModel
{
    public class CameraViewModel : ViewModelBase
    {
        private CameraService _cameraService;
        private ImageSource _videoSource { get; set; }
        private string _selectedCam;
        public string SelectedFilePath { get; set; }
        private string _fileNameJpg;
        private string _rtspUrl {  get; set; }
        private string _cameraName { get; set; }

        private List<Point> _cameraCoordinates = new List<Point>(); 
        public string CameraName
        {
            get => _cameraName;
            set
            {
                _cameraName = value;
                _cameraService.CameraName = value;
                OnPropertyChanged();
            }
        }

        private bool _useRedFiltr { get; set; }
        public bool UseRedFiltr
        {
            get => _useRedFiltr;
            set
            {
                _useRedFiltr = value;
                _cameraService.UseRedFiltr = value;
                OnPropertyChanged();
            }
        }
        private bool _useIpUrlPath;
        public bool UseIpUrlPath
        {
            get => _useIpUrlPath;
            set
            {
                _useIpUrlPath = value;
                _cameraService.UseIpUrlPath = value;
                OnPropertyChanged();
            }
        }

        #region UpperBound HSV: оттенк Hue, насыщенностью Saturation, яркость Value
        private int _upHue { get; set; }
        public int UpHue
        {
            get => _upHue;
            set
            {
                _upHue = value;
                _cameraService.UpHue = value;
                OnPropertyChanged();
            }
        }
        private int _upSaturation { get; set; }

        public int UpSaturation
        {
            get => _upSaturation;
            set
            {
                _upSaturation = value;
                _cameraService.UpSaturation = value;
                OnPropertyChanged();
            }
        }
        private int _upValue { get; set; }

        public int UpValue
        {
            get => _upValue;
            set
            {
                _upValue = value;
                _cameraService.UpValue = value;
                OnPropertyChanged();
            }
        }
        #endregion


        #region LowerBound HSV: оттенк Hue, насыщенностью Saturation, яркость Value
        private int _lowHue { get; set; }
        public int LowHue
        {
            get => _lowHue;
            set
            {
                _lowHue = value;
                _cameraService.LowHue = value;
                OnPropertyChanged();
            }
        }
        private int _lowSaturation { get; set; }

        public int LowSaturation
        {
            get => _lowSaturation;
            set
            {
                _lowSaturation = value;
                _cameraService.LowSaturation = value;
                OnPropertyChanged();
            }
        }
        private int _lowValue { get; set; }

        public int LowValue
        {
            get => _lowValue;
            set
            {
                _lowValue = value;
                _cameraService.LowValue = value;
                OnPropertyChanged();
            }
        }
        #endregion

        public string RtspUrl
        {
            get => _rtspUrl;
            set
            {
                _rtspUrl = value;
                _cameraService.RtspUrl = value;
                OnPropertyChanged();
            }
        }
        public string FileNameJpg
        {
            get => _fileNameJpg;
            set
            {
                _fileNameJpg = value;
                _cameraService.FileNameJpg = value;
                OnPropertyChanged();
            }
        }
        private string _recordColor = "White";
        public string RecordColor
        {
            get => _recordColor;
            set
            {
                _recordColor = value;
                OnPropertyChanged();
            }
        }
        private string _greenColor = "White";
        public string GreenColor
        {
            get => _greenColor;
            set
            {
                _greenColor = value;
                OnPropertyChanged();
            }
        }
        public string SelectedCam
        {
            get => _selectedCam;
            set
            {
                _selectedCam = value;
                _cameraService.SelectedCameraId = _allCams.IndexOf(_selectedCam);
            }
        }
        public ImageSource VideoSource

        {
            get => _videoSource;
            set
            {
                _videoSource = value;
                OnPropertyChanged();
            }
        }
        private List<string> _allCams = new List<string>();
        public List<string> AllCams { get => _allCams; set { _allCams = value; } }

        public CameraViewModel(CameraService cameraService)
        {
            _cameraService = cameraService;
            _cameraService.EventFromVideoCams += EventFromCamera;
            //_cameraService.EventFromVideoCamsT += EventFromCameraT;
            _cameraService = cameraService;
            _cameraService.InitCamera();
            _allCams = _cameraService.AllCams;
            if(_allCams.Count>0)
             _selectedCam = _allCams[0];


            #region Commands
            StopCommand = new LambdaCommand(
                OnButtonStopCommand,
                CanOnButtonStopCommand);
            OpenFileCommand = new LambdaCommand(
                OnButtonOpenFileCommand,
                CanOnButtonOpenFileCommand);
            ClosingCommand = new LambdaCommand(
                OnClosingCommand,
                CanClosingCommand);
            StartRecordCommand = new LambdaCommand(
                OnStartRecordCommand,
                CanStartRecordCommand);
            PauseCommand = new LambdaCommand(
                OnPauseCommand,
                CanPauseCommand);
            PlayCommand = new LambdaCommand(
                OnPlayCommand,
                CanPlayCommand);
            TakeScreenshot = new LambdaCommand(
                OnTakeScreenshot,
                CanTakeScreenshot);
            StopTakeScreenshot = new LambdaCommand(
               OnStopTakeScreenshot,
               CanStopTakeScreenshot);
            ViewImageMouseDownCommand = new LambdaCommand(
              OnViewImageMouseDownCommand,
              CanViewImageMouseDownCommand);
            #endregion
        }
        #region Commands
        #region ButtonStopCommand
        public ICommand StopCommand { get; }

        private async void OnButtonStopCommand(object p)
        {
            _cameraService.StartRecordVideo = false;
            _cameraService.StoptRecord();
            RecordColor = "White";
            GreenColor = "White";
            _cameraService.Stop = true;
            await _cameraService.CloseService();
            _cameraService.InitCamera();
        }
        private bool CanOnButtonStopCommand(object p) => true;
        #endregion
        #region ButtonStartRecordCommand
        public ICommand StartRecordCommand { get; }

        private void OnStartRecordCommand(object p)
        {
            _cameraService.CreateWriter();
            _cameraService.StartRecordVideo = true;
            RecordColor = "Red";
        }
        private bool CanStartRecordCommand(object p) => true;
        #endregion
        #region PauseCommand
        public ICommand PauseCommand { get; }

        private void OnPauseCommand(object p)
        {
            _cameraService.Pause = true;
            GreenColor = "White";
        }
        private bool CanPauseCommand(object p) => true;
        #endregion
        #region PlayCommand
        public ICommand PlayCommand { get; }

        private void OnPlayCommand(object p)
        {
            _cameraService.Pause = false;
            GreenColor = "Green";
        }
        private bool CanPlayCommand(object p) => true;
        #endregion

        #region TakeScreenshot
        public ICommand TakeScreenshot { get; }

        private void OnTakeScreenshot(object p)
        {
            _cameraService.TakeScreenshot(_fileNameJpg);
        }
        private bool CanTakeScreenshot(object p)
        {
            if (FileNameJpg == null || FileNameJpg == "")
                return false;
            else return true;
        }
        #endregion

        #region StpopTakeScreenshot
        public ICommand StopTakeScreenshot { get; }

        private void OnStopTakeScreenshot(object p)
        {
            _cameraService.StopTakeScreenshot();
        }
        private bool CanStopTakeScreenshot(object p)
        {
           return true;
        }
        #endregion

        #region ButtonOpenFileCommand
        public ICommand OpenFileCommand { get; }

        private void OnButtonOpenFileCommand(object p)
        {
            var openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                SelectedFilePath = openFileDialog.FileName;
                _cameraService.Stop = false;
                GreenColor = "Green";
                _cameraService.GoReadVideo(SelectedFilePath);
            }
        }
        private bool CanOnButtonOpenFileCommand(object p) => true;
        #endregion
        #region ClosingCommand
        public ICommand ClosingCommand { get; }
        private void OnClosingCommand(object p)
        {
           // _cameraService.CloseService();
        }

        public async Task<bool> CloseService()
        {
            var result = await _cameraService.CloseService();
            return result;
        }
        private bool CanClosingCommand(object p) => true;
        #endregion


        #region ViewImageMouseDownCommand
        public ICommand ViewImageMouseDownCommand { get; }
        private void OnViewImageMouseDownCommand(object p)
        {
            MouseEventArgs args = (MouseEventArgs)p;
            Image image = args.OriginalSource as Image;
            double imageWidth =(int)image.ActualWidth;
            double imageHeight =(int)image.ActualHeight;
            Point position = args.GetPosition((IInputElement)args.Source);
            double mouseX = (int)position.X;
            double mouseY = (int)  position.Y;
            _cameraService.AddPoint(mouseX, mouseY, imageWidth, imageHeight);
        }
        private bool CanViewImageMouseDownCommand(object p) => true;
        #endregion

        
        #endregion

        public void EventFromCamera(ImageSource video)
        {
            if (Application.Current != null )
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    VideoSource = video;
                });
            }
        }    
    }

    //public void EventFromCameraT(Mat video)
    //{
    //    if (Application.Current != null)
    //    {
    //        Application.Current.Dispatcher.Invoke(() =>
    //        {
    //            VideoSource = video.ToBitmapSource();
    //        });
    //    }
    //}
}

