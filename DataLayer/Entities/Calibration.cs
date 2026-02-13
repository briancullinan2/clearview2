using System;
using System.Collections.Generic;

namespace EPIC.DataLayer.Entities
{
    public class Calibration : IEntity
    {
        public int DeviceId { get; set; }
        public int CalibrationSettingId { get; set; }
        public DeviceCalibrationSetting CalibrationSetting { get; set; }
        public DataLayer.Entities.Image Image { get; set; }
        public DataLayer.Entities.Image Colorized { get; set; }
        public bool Failed { get; set; }
        public object NoiseLevel { get; set; }
        public DateTime TimeCalibrated { get; set; }
        public ICollection<DataLayer.Entities.Calibration> Calibrations { get; set; }
    }
}
