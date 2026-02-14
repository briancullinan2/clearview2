using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPIC.DataLayer.Entities
{
    public class Calibration : Entity<Calibration>
    {
        [Key]
        public int CalibrationId { get; set; }
        public int DeviceId { get; set; }
        public int CalibrationSettingId { get; set; }
        public int SettingId { get; set; }
        [ForeignKey(nameof(SettingId))]
        public DeviceCalibrationSetting CalibrationSetting { get; set; }
        public DataLayer.Entities.Image Image { get; set; }
        public DataLayer.Entities.Image Colorized { get; set; }
        public bool Failed { get; set; }
        public DateTime TimeCalibrated { get; set; }
        public virtual ICollection<ImageCalibration> Calibrations { get; set; }
    }
}
