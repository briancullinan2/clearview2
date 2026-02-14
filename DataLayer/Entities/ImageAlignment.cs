using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPIC.DataLayer.Entities
{
    public class ImageAlignment : Entity<ImageAlignment>
    {
        [Key]
        public int AlignmentId { get; set; }
        public double CenterX { get; set; }
        public double Angle { get; set; }
        public double RadiusX { get; set; }
        public double RadiusY { get; set; }
        public double CenterY { get; set; }
        public int FingerId { get; set; }
        [ForeignKey(nameof(FingerId))]
        public Image FingerImage { get; set; }
        public bool? Filtered { get; set; }
        public string Finger { get; set; }
        public int FingerSetId { get; set; }
        public int ImageId { get; set; }
        [ForeignKey(nameof(ImageId))]
        public Image Image { get; set; }
    }
}
