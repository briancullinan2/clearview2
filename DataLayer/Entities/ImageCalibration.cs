using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPIC.DataLayer.Entities
{
    [PrimaryKey(nameof(ImageId), nameof(CalibrationId))]
    [Table("IameCalibration")]
    public class ImageCalibration : Entity<ImageCalibration>
    {
        public bool CoronaFailed { get; set; }
        public int ImageId { get; set; }
        [ForeignKey(nameof(ImageId))]
        public Image Image { get; set; }
        public double IntensityTotal { get; set; }
        public double NoiseLevel { get; set; }
        public double IntensityInner { get; set; }
        public double IntensityCorona { get; set; }
        public double IntensityHighP { get; set; }
        public double IntensityOuter { get; set; }
        public double HighPMeanDiff { get; set; }
        public double CoronaMeanDiff { get; set; }
        public double InnerMeanDiff { get; set; }
        public double OuterMeanDiff { get; set; }
        public double TotalFailures { get; set; }
        public double HighPFailures { get; set; }
        public double CoronaFailures { get; set; }
        public double InnerFailures { get; set; }
        public double OuterFailures { get; set; }
        public double ClumpsFailures { get; set; }
        public double OuterTotalPixels { get; set; }
        public double InnerTotalPixels { get; set; }
        public double CoronaTotalPixels { get; set; }
        public double HighPTotalPixels { get; set; }
        public double TotalTotalPixels { get; set; }
        public double ClumpsTotalPixels { get; set; }
        public double OuterFailurePercent { get; set; }
        public double InnerFailurePercent { get; set; }
        public double CoronaFailurePercent { get; set; }
        public double HighPFailurePercent { get; set; }
        public double TotalFailurePercent { get; set; }
        public double ClumpsFailurePercent { get; set; }
        public bool OuterFailed { get; set; }
        public bool InnerFailed { get; set; }
        public bool HighPFailed { get; set; }
        public bool TotalFailed { get; set; }
        public bool ClumpsFailed { get; set; }
        public bool OuterMeanFailed { get; set; }
        public bool InnerMeanFailed { get; set; }
        public bool CoronaMeanFailed { get; set; }
        public bool HighPMeanFailed { get; set; }
        public bool Failed { get; set; }
        public int ColorizedId { get; set; }
        [ForeignKey(nameof(ColorizedId))]
        public Image Colorized { get; set; }
        public int CalibrationId { get; set; }
        [ForeignKey(nameof(CalibrationId))]
        [InverseProperty(nameof(Calibration.Calibrations))]
        public Calibration Calibration { get; set; }
        public int SettingId { get; set; }
        [ForeignKey(nameof(SettingId))]
        public DeviceCalibrationSetting CalibrationSetting { get; set; }
        public double SigmaRegionOuter { get; set; }
        public double SigmaRegionInner { get; set; }
        public double SigmaRegionCorona { get; set; }
        public double SigmaRegionHighP { get; set; }
        public double SigmaRegionTotal { get; set; }
        public double SigmaRegionClump { get; set; }
        public double ThresholdPercentOuter { get; set; }
        public double ThresholdPercentInner { get; set; }
        public double ThresholdPercentCorona { get; set; }
        public double ThresholdPercentHighP { get; set; }
        public double ThresholdPercentTotal { get; set; }
        public double ThresholdPercentClumps { get; set; }
        public double SigmaMeansOuter { get; set; }
        public double SigmaMeansInner { get; set; }
        public double SigmaMeansCorona { get; set; }
        public double SigmaMeansHighP { get; set; }
        public double SigmaMeansTotal { get; set; }
        public double SigmaMeansClumps { get; set; }
        public int BinDepth { get; set; }
    }
}
