using System.ComponentModel.DataAnnotations.Schema;

namespace EPIC.DataLayer.Entities
{
    [Table("DeviceSetting")]
    public class DeviceSetting : IEntity
    {
        public int Frequency { get; set; }
        public int PulseDuration { get; set; }
        public int PulseWidth { get; set; }
        public int ExposureDelay { get; set; }
        public int Voltage { get; set; }
        public int Brightness { get; set; }
        public int Gain { get; set; }
        public int DeviceId { get; set; }
        public int Id { get; set; }
    }
}
