using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.DataLayer.Entities
{
    public class Device
    {
        public bool IsDefault { get; set; }
        public object DeviceId { get; set; }
        public string UniqueIdentifier { get; set; }
        public int UidQualifier { get; set; }
        public int DeviceState { get; set; }
        public string SerialNumber { get; set; }
        public DateTime DateIssued { get; set; }
        public string RevisionLevel { get; set; }
        public int ScansAvailable { get; set; }
        public int ScansCompleted { get; set; }
        public DateTime? LastActivityTime { get; set; }
        public string Firmware { get; set; }
        public string Camera { get; set; }
        public IEnumerable<DeviceSetting> Settings { get; set; }
        public IEnumerable<DeviceCalibrationSetting> CalibrationSettings { get; set; }
    }
}
