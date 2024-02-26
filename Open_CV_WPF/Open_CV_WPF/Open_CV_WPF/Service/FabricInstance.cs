using DAL;
using Microsoft.Extensions.DependencyInjection;
using Open_CV_WPF.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Open_CV_WPF.Service
{
    public class FabricInstance : IFabricInstance
    {
        private IDataWorker _dataWorker;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private List<CameraViewModel> _instancesCamViewModel = new List<CameraViewModel>();
        public List <CameraViewModel> InstancesCamViewModel 
        {
            get => _instancesCamViewModel; 
            set => _instancesCamViewModel =value;
        }
     

        public void Init()
        {

            var allSeting = _dataWorker.ReadSetingFromContext().ToList();

            for (var i = 0; i <= 1; i++)
            {
                using (var scope1 = _serviceScopeFactory.CreateScope())
                {
                    // Получение первого экземпляра сервиса
                    var instance = scope1.ServiceProvider.GetRequiredService<CameraViewModel>();
                    instance.CameraName = allSeting[i].CameraName;
                    instance.RtspUrl = allSeting[i].RtspUrl;
                    instance.FileNameJpg = allSeting[i].FileNameJpg;
                    instance.UpHue = allSeting[i].UpHue;
                    instance.UpSaturation = allSeting[i].UpSaturation;
                    instance.UpValue = allSeting[i].UpValue;
                    instance.LowHue = allSeting[i].LowHue;
                    instance.LowSaturation = allSeting[i].LowSaturation;
                    instance.LowValue = allSeting[i].LowValue;
                    instance.UseIpUrlPath = allSeting[i].UseIpUrlPath;
                    instance.UseRedFiltr = allSeting[i].UseRedFiltr;
                    _instancesCamViewModel.Add(instance);
                }

            }
        }

        public FabricInstance(IDataWorker dataWorker, IServiceScopeFactory serviceScopeFactory)
        {
            _dataWorker = dataWorker;
            _serviceScopeFactory = serviceScopeFactory;
        }
    }
}
