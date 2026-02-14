using System;
using System.Collections.Generic;

namespace EPIC.DataLayer.Entities
{
    public class FingerSet : Entity<FingerSet>
    {
        public int FingerSetId { get; set; }
        public Calibration Calibration { get; set; }
        public string SoftwareVersion { get; set; }
        public DateTime TimeScanned { get; set; }
        public ICollection<DataLayer.Entities.ImageAlignment> Images { get; set; }
    }
}
