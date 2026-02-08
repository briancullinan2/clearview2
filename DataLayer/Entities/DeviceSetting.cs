using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.DataLayer.Entities
{
    public class DeviceSetting
    {
        public int Frequency { get; set; }
        public int PulseDuration { get; set; }
        public int PulseWidth { get; set; }
        public int ExposureDelay { get; set; }
        public int Voltage { get; set; }
        public int Brightness { get; set; }
        public int Gain { get; set; }
        public object DeviceId { get; set; }
    }
}
