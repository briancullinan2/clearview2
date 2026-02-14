using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPIC.DataLayer.Entities
{
    [Table("FingerSet")]
    public class FingerSet : Entity<FingerSet>
    {
        public int FingerSetId { get; set; }
        public Calibration Calibration { get; set; }
        public string SoftwareVersion { get; set; }
        public DateTime TimeScanned { get; set; }
        public ICollection<DataLayer.Entities.ImageAlignment> Images { get; set; }
    }
}
