using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPIC.DataLayer.Entities
{
    [Table("Device")]
    public class Device : IEntity
    {
        public bool IsDefault { get; set; }
        public int DeviceId { get; set; }
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
        public int Height { get; set; }
        public int Width { get; set; }
        public IEnumerable<DeviceSetting> Settings { get; set; }
        public IEnumerable<DeviceCalibrationSetting> CalibrationSettings { get; set; }
    }
}
