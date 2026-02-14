using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPIC.DataLayer.Entities
{
    [PrimaryKey(nameof(ImageId), nameof(CaptureId))]
    [Table("ImageCapture")]
    public class ImageCapture : Entity<ImageCapture>
    {
        public int CaptureId { get; set; }
        public Capture Capture { get; set; }
        public int ImageId { get; set; }
        public Image Image { get; set; }
    }
}
