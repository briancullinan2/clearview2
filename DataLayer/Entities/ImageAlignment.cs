namespace EPIC.DataLayer.Entities
{
    public class ImageAlignment : IEntity
    {
        public int CenterX { get; set; }
        public float Angle { get; set; }
        public double RadiusX { get; set; }
        public double RadiusY { get; set; }
        public int CenterY { get; set; }
        public Image FingerImage { get; set; }
        public bool? Filtered { get; set; }
        public string Finger { get; set; }
        public object ImageId { get; set; }
        public DataLayer.Entities.Image Image { get; set; }
    }
}
