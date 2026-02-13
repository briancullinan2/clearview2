using System.Collections.Generic;

namespace EPIC.DataLayer.Entities
{
    public class Image : IEntity
    {
        public DataLayer.Entities.ImageCapture Capture { get; set; }
        public byte[] ImageData { get; set; }
        public DataLayer.Entities.ImageAlignment ImageAlignment { get; set; }
        public object ImageId { get; set; }
        public IEnumerable<DataLayer.Entities.ImageSector> ImageSectors { get; set; }
    }
}
