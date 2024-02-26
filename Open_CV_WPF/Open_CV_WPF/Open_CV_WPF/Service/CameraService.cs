using DAL;
using DirectShowLib;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;



namespace Open_CV_WPF.Service
{
    public class CameraService
    {
        public string CameraName { get; set; }  
        public string RtspUrl { get; set; }
        public bool UseIpUrlPath {  get; set; } 
        public string FileNameJpg { get; set; }    
        private VideoCapture _capture;
        private Mat _frame;
        private DsDevice[] _webCams;
        private int _selectedCameraId;
        private Mat _videoSource;
        private Mat _oldFrame;
        private int _frameWidth;
        private int _frameHeight;
        private VideoWriter _writer;
        public List<string> AllCams = new List<string>();
        public bool StartRecordVideo;
        public bool Pause;
        public bool Stop;
        private CancellationTokenSource cancelTokenSource;
        private CancellationTokenSource cancelTokenScreenShot;
        public bool UseRedFiltr {  get; set; }  
        public int UpHue {  get; set; }
        public int UpSaturation { get; set; }
        public int UpValue { get; set; }
        public int LowHue { get; set; }
        public int LowSaturation { get; set; }
        public int LowValue { get; set; }
        public double VideoLength {  get; set; }  
        public double VideoPosFrames { get; set; }
        public event Action<ImageSource> EventFromVideoCams;
        public event Action<Mat> EventFromVideoCamsT;
        private bool goGrab;
        private bool DetectRedObjectsProces;
        private IDataWorker _dataWorker;
        public List<Point> CameraCoordinates { get => _cameraCoordinates; set => _cameraCoordinates = value; }
        private List<Point> _cameraCoordinates;

        public void AddPoint(double x, double y, double imageWidth, double imageHeight)
        {
            var xWidth = imageWidth / _frameWidth;
            var xHeight = imageHeight / _frameHeight;
            var newX =(int)(x / xWidth);
            var newY = (int)(y / xHeight);
            var point = new Point(newX, newY);
            _cameraCoordinates.Add(point);
        }
        public CameraService(IDataWorker dataWorker)
        {
            _dataWorker = dataWorker;
            _cameraCoordinates= new List<Point>();
        }
        public int SelectedCameraId
        {
            get => _selectedCameraId;

            set
            {
                _selectedCameraId = value;
                _capture.Stop();
                InitCamera();
            }
        }

        Task<bool> СonnectTask = Task.Run(() =>
        {
            try
            {
                // Попытка подключения к камере
                string rtspUrl = "rtsp://admin:admin@192.168.88.100:554/video";
                VideoCapture capture = new VideoCapture(rtspUrl);
                capture.Dispose(); // Освобождение ресурсов захвата видео
                return true; // Подключение успешно
            }
            catch
            {
                Debug.WriteLine("Camera is not aviablel");
                return false; // Подключение не удалось
            }
        });

        public async void InitCamera()
        {
            _webCams = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);
            var test = _webCams;

            if (_capture != null)
            {
                _capture.Dispose();
                Stop = true;
            }


            if (_webCams != null)
            {
                string rtspUrl = "rtsp://admin:admin@192.168.88.100:554/video";
                CvInvoke.UseOpenCL = false;
                AllCams = _webCams.Select(p => p.Name).ToList();
                //  _capture = new VideoCapture(_selectedCameraId,VideoCapture.API.DShow);
                bool isConnected = await Task.WhenAny(СonnectTask, Task.Delay(5000)) == СonnectTask;
                if (isConnected) 
                {
                    _capture = new VideoCapture(rtspUrl);
                }
                else
                {
                    _capture = new VideoCapture(_selectedCameraId, VideoCapture.API.DShow);
                    Debug.WriteLine("Camera is  aviablel");
                }
                
                if (_capture.IsOpened)
                {
                    _frameWidth = (int)_capture.Get(Emgu.CV.CvEnum.CapProp.FrameWidth);
                    _frameHeight = (int)_capture.Get(Emgu.CV.CvEnum.CapProp.FrameHeight);
                    _capture.ImageGrabbed += ProcessFrame;
                    _capture.Start();
                    int desiredFPS = 1;
                    double frameDelayMilliseconds = 1000 / desiredFPS;
                    cancelTokenSource = new CancellationTokenSource();
                    cancelTokenScreenShot = new CancellationTokenSource();
                    Task backgroundTask = Delay(cancelTokenSource.Token);
                }
            }
        }
        private async void ProcessFrame(object sender, EventArgs e)
        {
            if (_capture != null && _capture.Ptr != IntPtr.Zero && goGrab == true && !DetectRedObjectsProces)
            {
                using (_frame = new Mat())
                {
                    _capture.Retrieve(_frame, 0);
                    string currentDate = DateTime.Now.ToString();
                    goGrab = false;
                   // CvInvoke.CvtColor(_frame, _frame, ColorConversion.Bgr2Gray);//Чернобелое 
                   // CvInvoke.AdaptiveThreshold(_frame, _frame, UpHue, AdaptiveThresholdType.GaussianC, ThresholdType.Binary,11,2);//Применение поргоговых хначений цвета 
                   //  CvInvoke.BitwiseNot(_frame, _frame); //Инверсия цывета
                    // CvInvoke.Threshold(_frame, _frame, UpHue, UpSaturation, ThresholdType.Binary);//Применение поргоговых хначений цвета 
                    
                    
                    // CvInvoke.ConvertScaleAbs(_frame, _frame, 1,1);
                    // CvInvoke.InRange(_frame, new ScalarArray(new MCvScalar(0, 0, 100)), new ScalarArray(new MCvScalar(100, 100, 255)), _frame);
                    CvInvoke.PutText(_frame, currentDate, new System.Drawing.Point(1500, 800),
                    FontFace.HersheyComplex, 1, new MCvScalar(125, 225, 155), 2);
                    if (UseRedFiltr)
                        _frame = DetectRedObjects(_frame);
                    _oldFrame = _frame.Clone();
                    if (StartRecordVideo)
                        StartRecord(_frame);
                    _frame = GridNet(_frame);

                    if (ToBitmapSource(_frame) != null);
                        EventFromVideoCams?.Invoke(ToBitmapSource(_frame));
                     //EventFromVideoCamsT.Invoke(_frame);
                }
            }
        }

        async Task Delay(CancellationToken cancellationToken)
        {
            await Task.Run(() =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    goGrab = true;
                    Thread.Sleep(100);
                }

            }, cancellationToken);
        }

        // Конвертирует Mat в BitmapSource
        private ImageSource ToBitmapSource(Mat image)
        {
            using (var bitmap = image.ToBitmap())
            {
                using (var stream = new MemoryStream())
                {
                    if (bitmap != null)
                    bitmap.Save(stream, ImageFormat.Bmp);
                    stream.Seek(0, SeekOrigin.Begin);

                    var bitmapSource = BitmapFrame.Create(stream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
                    return bitmapSource;
                }
            }
        }

        private Mat DetectRedObjects(Mat input)
        {
            DetectRedObjectsProces = true;
            // Конвертируем изображение из формата BGR в HSV
            using (Mat hsvImage = new Mat())
            {
                CvInvoke.CvtColor(input, hsvImage, ColorConversion.Bgr2Hsv);

                 //Определяем  диапазон в HSV
                 ScalarArray upperRed = new ScalarArray(new MCvScalar(UpHue, UpSaturation, UpValue));
                 ScalarArray lowerRed = new ScalarArray(new MCvScalar(LowHue, LowSaturation, LowValue));
       
                // Создаем маску 
                Mat redMask = new Mat();
                CvInvoke.InRange(hsvImage, lowerRed, upperRed, redMask);
                 hsvImage.Dispose();
                DetectRedObjectsProces = false;
                return redMask;
            }             
        }

        public void CreateWriter ()
        {

              _writer = new VideoWriter($"{DateTime.Now.ToString("yyyy-MM-dd-ss")}.avi", VideoWriter.Fourcc('M', 'J', 'P', 'G'),
              30, new System.Drawing.Size(_frameWidth, _frameHeight), true);
        }
        public void StartRecord(Mat frame)
        {
                _writer.Write(frame);
      
        }

        public void StoptRecord()
        {
            if ( _writer != null ) 
            {
                _writer.Dispose();
            }
        }


        private async Task SaveDataToDatabase()
        {
            await Task.Run(() =>
            {
                _dataWorker.WriteSetingFromContext(new MyAppSeting
                {
                    CameraName = CameraName,
                    RtspUrl = RtspUrl,
                    UpHue = UpHue,
                    UpSaturation = UpSaturation,
                    UpValue = UpValue,
                    LowHue = LowHue,
                    LowSaturation = LowSaturation,
                    LowValue = LowValue,
                    FileNameJpg = FileNameJpg,
                    UseIpUrlPath = UseIpUrlPath,
                    UseRedFiltr = UseRedFiltr,  
                });
            });        
        }

        public async Task <bool>  CloseService()
        {
            await SaveDataToDatabase();
            cancelTokenScreenShot.Cancel();
            cancelTokenSource.Cancel();
            Stop = true;
            return true;
        }

        public void GoReadVideo(string SelectedFilePath)
        {
            _capture.Stop();

            if (_webCams != null)
            {
                using (var capture = new VideoCapture(SelectedFilePath))
                {
                    // Проверяем, удалось ли открыть видеофайл
                    if (!capture.IsOpened)
                    {
                        Console.WriteLine("Не удалось открыть видеофайл.");
                        return;
                    }
                    VideoLength = capture.Get(Emgu.CV.CvEnum.CapProp.FrameCount);

                    while (!Stop)
                    {

                        VideoPosFrames = capture.Get(Emgu.CV.CvEnum.CapProp.PosFrames);
                        
                        if (!Pause)
                        {
                            Mat frame = new Mat();
                            capture.Read(frame); ; // Считываем кадр из видеофайла
                            if (frame.IsEmpty) // Если кадр пустой, значит видео закончилось
                                break;
                            if (UseRedFiltr)
                                frame = DetectRedObjects(frame);               
                            //   CvInvoke.CvtColor(frame, frame, ColorConversion.Bgr2Gray);//Чернобелое 
                            //   CvInvoke.AdaptiveThreshold(frame, frame, UpHue, AdaptiveThresholdType.GaussianC, ThresholdType.Binary, 11, 2);//Применение поргоговых хначений цвета 
                            EventFromVideoCams.Invoke(ToBitmapSource(frame));
                            _oldFrame = frame;
                        }
                        else
                        {                           
                            EventFromVideoCams.Invoke(ToBitmapSource(_oldFrame));
                        }
                     
                        // Ожидаем нажатия клавиши для выхода
                        if (Emgu.CV.CvInvoke.WaitKey(30) >= 0)
                            break;
                    }
                }
            }
        }
        public Mat CatSizeVideo (Mat frame)
        {
            int x = 100;
            int y = 100;
            int width = 300;
            int height = 250;
            Rectangle roi = new Rectangle(x, y, width, height);
            frame = new Mat(frame, roi);
            return frame;
        }
        

        async Task TakeAutomaticScreenShotRun(CancellationToken cancellationToken, string FileName)
        {
            await Task.Run(() =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    TakeScreenshotAction(FileName);
                    Thread.Sleep(2000);
                }

            }, cancellationToken);
        }

        public void TakeScreenshot (string FileName) 
        {
            if (_oldFrame != null && FileName !=null)
            {
                FileNameJpg = FileName;
                cancelTokenScreenShot = new CancellationTokenSource();
                Task TaskTakeScreenshot = TakeAutomaticScreenShotRun(cancelTokenScreenShot.Token, FileName);
            }
        }

        public void TakeScreenshotAction(string FileName)
        {
            if (_oldFrame != null && FileName != null)
            {
                var myFolder = FileName;
                var location = Directory.GetCurrentDirectory();
                var allDir = Directory.GetDirectories(location);
                DirectoryInfo myDir = new DirectoryInfo($"{location}\\{myFolder}");
                if (!allDir.Contains($"{location}\\{myFolder}"))
                {
                    myDir.Create();
                }
                CvInvoke.Imwrite($"{location}\\{myFolder}\\{FileName}{DateTime.Now.ToString("HH-mm-ss")}.jpg", _oldFrame);
            }
        }
        public void StopTakeScreenshot()
        {
            cancelTokenScreenShot.Cancel();
        }

        public Mat GridNet(Mat mat)
        {
            // Преобразование Mat в Image<Bgr, byte>
            Image<Bgra, byte> emguImage = mat.ToImage<Bgra, byte>();
            List<Rectangle> rectangles = new List<Rectangle>();
            for (int v = 0; v < emguImage.Height; v += 48)
            {
                for (int i = 0; i < emguImage.Width; i += 64)
                {
                    rectangles.Add(new Rectangle(i, v, 64, 48));
                }
            }
            foreach (Rectangle rect in rectangles)
            {
                if (ChekSelectedRect (rect.X, rect.Y))
                {
                    using (Image<Bgra, byte> tempImage = new Image<Bgra, byte>(emguImage.Width, emguImage.Height, new Bgra(0, 0, 0, 0)))
                    {
                        tempImage.Draw(rect, new Bgra(0, 255, 0, 128), -1); // Рисование прямоугольника с полупрозрачным заполнением
                        emguImage = emguImage.AddWeighted(tempImage, 1, 1, 0); // Смешивание с учетом альфа-канала
                    }                      
                }
                else
                {
                    emguImage.Draw(rect, new Bgra(22, 250, 0, 1), 1); // Рисование прямоугольников на изображении       
                }               
            }
            var resultImage = emguImage.Mat;
            return resultImage;    
        }

        public bool ChekSelectedRect(int x, int y)
        {
            bool contain = false;
            _cameraCoordinates.ForEach(point =>
            {
                if (point.X > x && point.X < x + 64 && point.Y > y && point.Y < y + 48)
                    contain = true;
            });
            return contain;          
        }
    }

}