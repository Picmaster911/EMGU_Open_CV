using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class MyAppSeting
    {
        public int Id { get; set; }
        public string  CameraName { get; set; }
        public string RtspUrl { get; set; }
        public bool UseIpUrlPath { get; set; }  
        public bool UseRedFiltr { get; set; }
        public int UpHue { get; set; }
        public int UpSaturation { get; set; }

        public int UpValue { get; set; }
        public int LowHue { get; set; }
        public int LowSaturation { get; set; }
        public int LowValue { get; set; }
        public string FileNameJpg { get; set; } 
    }
}
