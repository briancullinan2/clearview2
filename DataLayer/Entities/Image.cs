using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPIC.DataLayer.Entities
{
    public class Image : Entity<Image>
    {
        public DataLayer.Entities.ImageCapture Capture { get; set; }
        public byte[] ImageData { get; set; }

        public int AlignmentId { get; set; }
        //[ForeignKey(nameof(AlignmentId))]
        [InverseProperty(nameof(ImageAlignment.FingerImage))]
        public ImageAlignment FingerAlignment { get; set; }
        [Key]
        public int ImageId { get; set; }
        public IEnumerable<ImageSector> ImageSectors { get; set; }
    }
}
