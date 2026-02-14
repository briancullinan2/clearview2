using System.ComponentModel.DataAnnotations.Schema;

namespace EPIC.DataLayer.Entities
{
    [Table("DeviceSetting")]
    public class DeviceSetting : Entity<DeviceSetting>
    {
        public Customization.PWM0Frequency Frequency { get; set; }
        public Customization.PulseDuration PulseDuration { get; set; }
        public Customization.PulseWidth PulseWidth { get; set; }
        public int ExposureDelay { get; set; }
        public Customization.Voltage Voltage { get; set; }
        public int Brightness { get; set; }
        public int Gain { get; set; }
        public int DeviceId { get; set; }
        public int Id { get; set; }
        public bool IsDefault { get; set; }
        public Entities.Device Device { get; set; }
    }
}
