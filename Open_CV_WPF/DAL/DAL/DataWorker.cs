

using System.Net.Http;

namespace DAL
{
    public class DataWorker : IDataWorker
    {
        private readonly AppDbContext _appDbContext;
        public DataWorker(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public List<MyAppSeting> ReadSetingFromContext()
        {
            var appSetings = _appDbContext.MyAppSetings.ToList();

            return appSetings;
        }

        public void WriteSetingFromContext(MyAppSeting appSeting)
        {
            var settingsList = _appDbContext.MyAppSetings.ToList();
            if (appSeting.CameraName == "Camera1")
            {
                test(settingsList, "Camera1", appSeting);

            }
            if (appSeting.CameraName == "Camera2")
            {
                test(settingsList, "Camera2", appSeting);
            }

            _appDbContext.SaveChanges();
            
        }
        private void test (List<MyAppSeting> settingsList, string nameCamera, MyAppSeting appSeting)
        {
            var cameraSeting = settingsList.Where(x => x.CameraName == nameCamera).FirstOrDefault();
            if (appSeting.RtspUrl != null)
                cameraSeting.RtspUrl = appSeting.RtspUrl;

            if (appSeting.FileNameJpg != null)
                cameraSeting.FileNameJpg = appSeting.FileNameJpg;

            if (appSeting.UpHue != null)
                cameraSeting.UpHue = appSeting.UpHue;

            if (appSeting.UpSaturation != null)
                cameraSeting.UpSaturation = appSeting.UpSaturation;

            if (appSeting.UpValue != null)
                cameraSeting.UpValue = appSeting.UpValue;

            if (appSeting.LowHue != null)
                cameraSeting.LowHue = appSeting.LowHue;

            if (appSeting.LowSaturation != null)
                cameraSeting.LowSaturation = appSeting.LowSaturation;

            if (appSeting.LowValue != null)
                cameraSeting.LowValue = appSeting.LowValue;

            if (appSeting.UseIpUrlPath != null)
                cameraSeting.UseIpUrlPath = appSeting.UseIpUrlPath;

            if (appSeting.UseRedFiltr != null)
                cameraSeting.UseRedFiltr = appSeting.UseRedFiltr;
        }

    }
}
