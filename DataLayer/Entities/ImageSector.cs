using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPIC.DataLayer.Entities
{
    [PrimaryKey(nameof(ImageId), nameof(SectorNumber))]
    [Table("ImageSector")]
    public class ImageSector : Entity<ImageSector>
    {
        public int ImageId { get; set; }
        [ForeignKey(nameof(ImageId))]
        public Image Image { get; set; }
        public short SectorNumber { get; set; }
        public double StartAngle { get; set; }
        public double EndAngle { get; set; }
        public double IntegralArea { get; set; }
        public double SectorArea { get; set; }
        public double NormalizedArea { get; set; }
        public double Entropy { get; set; }
        public double FractalCoefficient { get; set; }
        public double JsInteger { get; set; }
        public double AverageIntensity { get; set; }
        public double FormCoefficient { get; set; }
        public double Form2 { get; set; }
        public double Form11 { get; set; }
        public double Form12 { get; set; }
        public double Form13 { get; set; }
        public double Form14 { get; set; }
        public double Form2Prime { get; set; }
        public double Ai1 { get; set; }
        public double Ai2 { get; set; }
        public double Ai3 { get; set; }
        public double Ai4 { get; set; }
        public double BreakCoefficient { get; set; }
        public double RingThickness { get; set; }
        public double RingIntensity { get; set; }
    }
}
