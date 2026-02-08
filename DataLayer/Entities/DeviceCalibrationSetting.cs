using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.DataLayer.Entities
{
    public class DeviceCalibrationSetting
    {
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
        public object DeviceId { get; set; }
    }
}
