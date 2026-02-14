using Microsoft.EntityFrameworkCore;

namespace EPIC.DataLayer.Entities
{
    [PrimaryKey(nameof(ImageId), nameof(CaptureId))]
    public class ImageCapture : Entity<ImageCapture>
    {
        public int CaptureId { get; set; }
        public Capture Capture { get; set; }
        public int ImageId { get; set; }
        public Image Image { get; set; }
    }
}
