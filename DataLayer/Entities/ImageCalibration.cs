namespace EPIC.DataLayer.Entities
{
    public class ImageCalibration : IEntity
    {
        public bool CoronaFailed { get; set; }
        public DataLayer.Entities.Image Image { get; set; }
        public float IntensityTotal { get; set; }
        public float NoiseLevel { get; set; }
        public float IntensityInner { get; set; }
        public float IntensityCorona { get; set; }
        public float IntensityHighP { get; set; }
        public float IntensityOuter { get; set; }
        public float HighPMeanDiff { get; set; }
        public float CoronaMeanDiff { get; set; }
        public float InnerMeanDiff { get; set; }
        public float OuterMeanDiff { get; set; }
        public float TotalFailures { get; set; }
        public float HighPFailures { get; set; }
        public float CoronaFailures { get; set; }
        public float InnerFailures { get; set; }
        public float OuterFailures { get; set; }
        public float ClumpsFailures { get; set; }
        public float OuterTotalPixels { get; set; }
        public float InnerTotalPixels { get; set; }
        public float CoronaTotalPixels { get; set; }
        public float HighPTotalPixels { get; set; }
        public float TotalTotalPixels { get; set; }
        public float ClumpsTotalPixels { get; set; }
        public float OuterFailurePercent { get; set; }
        public float InnerFailurePercent { get; set; }
        public float CoronaFailurePercent { get; set; }
        public float HighPFailurePercent { get; set; }
        public float TotalFailurePercent { get; set; }
        public float ClumpsFailurePercent { get; set; }
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
        public DataLayer.Entities.Image Colorized { get; set; }
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
