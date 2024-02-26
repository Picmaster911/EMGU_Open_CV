using Open_CV_WPF.ViewModel;

namespace Open_CV_WPF.Service
{
    public interface IFabricInstance
    {
        List<CameraViewModel> InstancesCamViewModel { get; set; }
        void Init();
    }
}