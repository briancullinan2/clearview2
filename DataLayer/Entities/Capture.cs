using EPIC.DataLayer.Customization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EPIC.DataLayer.Entities
{
    public class Capture : Entity<Capture>
    {
        [Key]
        public int CaptureId { get; set; }
        public int Brightness { get; set; }
        public int Gain { get; set; }
        public Voltage Voltage { get; set; }
        public DateTime CaptureTime { get; set; }
        public int DeviceId { get; set; }
        public PWM0Frequency Frequency { get; set; }
        public PulseDuration PulseDuration { get; set; }
        public PulseWidth PulseWidth { get; set; }
        public int ExposureDelay { get; set; }
        public int Exposure { get; set; }
        public Image Image { get; set; }
        public ICollection<DataLayer.Entities.ImageCapture> Images { get; set; }
    }
}
