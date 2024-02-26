using Open_CV_WPF.Service;

namespace Open_CV_WPF.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        private IFabricInstance _fabricInstance;
        public CameraViewModel CameraFirstViewModel { get; }
        public CameraViewModel CameraSecondViewModel { get; }
        public MainWindowViewModel(IFabricInstance _fabricInstance)
        {
            CameraFirstViewModel = _fabricInstance.InstancesCamViewModel[0];
            CameraSecondViewModel = _fabricInstance.InstancesCamViewModel[1];
        }
    }
}
